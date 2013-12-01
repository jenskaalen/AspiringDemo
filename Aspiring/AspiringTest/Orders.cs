//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Management.Instrumentation;
using AspiringDemo;
using System.IO;
using System.Linq;
using AspiringDemo.ANN;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using AspiringDemo.Pathfinding;
using AspiringDemo.Saving;
using AspiringDemo.Sites;
using AspiringDemo.Factions;
using AspiringDemo.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Orders
    {
        IKernel _kernel = new StandardKernel(new NinFactory());

        [TestInitialize]
        public void Initialize()
        {
            GameFrame.Game.ObjectFactory = new TestSave("asd");
            GameFrame.Game.ZonePathfinder = new Pathfinder<IZone>();
            GameFrame.Game.ZonePathfinder.Nodes = new List<IZone>();
        }

        [TestMethod]
        public void RegisterOrder()
        {
            IFaction faction = new Faction();
            var unit = new Unit(faction);
            var orderMock = new Mock<IUnitOrder>();

            unit.AssignOrder(orderMock.Object);

            Assert.AreEqual(orderMock.Object, unit.Order);
        }

        [TestMethod]
        public void Area_Guarding_Priority()
        {
            var save = new TestSave("asd");
            var faction = _kernel.Get<IFaction>();
            var zone = new Zone(0,0, 499, 499);
            var area = new PopulatedArea(faction, null); //_kernel.Get<IPopulatedArea>();

            zone.AddArea(area);
            GameFrame.Game.ZonePathfinder.Nodes.Add(zone);
            faction.Initialize();

            var unitmock = new Mock<IUnit>(); 
            unitmock.Setup(unit => unit.Zone).Returns(zone);
            unitmock.Setup(unit => unit.Faction).Returns(faction);
            unitmock.Setup(unit => unit.Order).Returns(new GuardAreaOrder(area, unitmock.Object));
            
            var unit1 = unitmock.Object;

            Assert.AreEqual(unit1.Order.GetType(), typeof(GuardAreaOrder));
            Assert.AreEqual(unit1.Faction, faction);

            unit1.EnterZone(zone);
            area.Owner = faction;
            faction.Army.Units.Add(unit1);

            var manager = faction.FactionManager.UnitManager;
            //var manager = _kernel.Get<IUnitManager>(new ConstructorArgument("faction", faction));

            double prio = manager.GetAreaGuardPriority(area);
            Assert.IsTrue(prio == 2);

            unitmock.Setup(unit => unit.Order).Returns((IUnitOrder)null);
            prio = manager.GetAreaGuardPriority(area);
            Assert.IsTrue(prio == 3);
        }

        [TestMethod]
        public void Give_Guard_Area_Order()
        {
            var areaZone = new Zone(0, 0, 499, 499);
            var startZone = new Zone(500, 500, 499, 499);
            GameFrame.Game.ZonePathfinder.Nodes.Add(areaZone);
            GameFrame.Game.ZonePathfinder.Nodes.Add(startZone);
            areaZone.Neighbours = new List<IZone>() { startZone };
            startZone.Neighbours = new List<IZone>() { areaZone };
            var faction = _kernel.Get<IFaction>();
            var area = new PopulatedArea(faction, null);
            var unit1 = _kernel.Get<IUnit>();
            unit1.Zone = startZone;

            areaZone.AddArea(area);
            IUnitOrder order = new GuardAreaOrder(area, unit1);
            
            unit1.AssignOrder(order);

            Assert.AreEqual(unit1.Order, order);

            order.Execute();

            Assert.AreEqual(unit1.State, UnitState.ExecutingOrder);
            Assert.AreNotEqual(areaZone, unit1.Zone);
            Assert.AreEqual(startZone, unit1.Zone);

            order.Update(1f);

            Assert.AreNotEqual( startZone, unit1.Zone);
            Assert.AreEqual(areaZone, unit1.Zone);
            Assert.AreEqual(UnitState.ExecutingOrder, unit1.State);
        }

        [TestMethod]
        public void AttackAction()
        {
            var faction = _kernel.Get<IFaction>();
            GameFrame.Game.ZonePathfinder.Nodes = Factories.GetZones(10, 10);

            IZone startZone = GameFrame.Game.ZonePathfinder.Nodes[0];
            IZone z1 = GameFrame.Game.ZonePathfinder.Nodes[1];
            IZone z2 = GameFrame.Game.ZonePathfinder.Nodes[2];

            var action = new AttackAction(faction, z2, z1, 3);

            // should return false if there are no units..
            Assert.IsFalse(action.IsEveryoneInGatherZone());

            ISquad squad = faction.CreateSquad();
            squad.AddMember(faction.Create<Unit>());
            squad.AddMember(faction.Create<Unit>());
            squad.AddMember(faction.Create<Unit>());

            foreach (var member in squad.Members)
                member.Zone = startZone;

            Assert.AreEqual(3, faction.Army.Units.Count);

            action.AddSquad(squad);
            Assert.AreEqual(1, action.Squads.Count);
            Assert.AreEqual(3, action.MemberCount);
            Assert.AreEqual(squad.State, SquadState.Idle);

            action.Work();
            squad.Members.ForEach(x => x.Order.Execute());
            Assert.AreEqual(SquadState.ExecutingOrder, squad.State);

            squad.Members.ForEach(x => x.Order.Update(1f));
            squad.Members.ForEach(x => x.Order.Update(2f));
            Assert.AreEqual(3, squad.Members.Count(unit => unit.State == UnitState.Waiting));

            action.Work();
            Assert.IsTrue(action.IsEveryoneInGatherZone());

            squad.Members.ForEach(x => x.Order.Update(3f));
            Assert.AreEqual(3, squad.Members.Count(unit => unit.State == UnitState.ExecutingOrder));

            squad.Members.ForEach(x => x.Order.Update(4f));
            Assert.AreEqual(3, squad.Members.Count(unit => unit.State == UnitState.Idle));
        }

        [TestMethod]
        public void Squad_Is_In_Gather_Zone()
        {
            ISquad sq = new Squad();
            IUnit unit1 = new Unit(new Faction());
            sq.AddMember(unit1);

            IZone gatherZone = new Zone(0,0, 499, 499);
            IZone targetZone = new Zone(500, 500, 499, 499); 
            sq.EnterZone(gatherZone);

            var action = new AttackAction(new Faction(), targetZone, gatherZone, 1);
            action.AddSquad(sq);

            Assert.IsTrue(action.IsEveryoneInGatherZone());
        }

        [TestMethod]
        public void Helper_TravelZone_Retreat()
        {
            var zones = Factories.GetZones(9, 9);
            GameFrame.Game.ZonePathfinder.Nodes = zones;

            IFaction faction = GameFrame.Game.Factory.Get<IFaction>();
            IUnit unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));

            var gametimeMock = new Mock<IGameTime>();
            gametimeMock.Setup(time => time.Time).Returns(1);

            IGameTime gametime = gametimeMock.Object;

            GameFrame.Game.GameTime = gametime;
            
            unit.EnterZone(zones[0]);
            AspiringDemo.Gamecore.Helpers.Actions.GiveRetreatOrder(unit, zones[4]);
            Assert.IsTrue(unit.Order != null);
            Assert.AreEqual(unit.Zone, zones[1]);
        }
    }
}
