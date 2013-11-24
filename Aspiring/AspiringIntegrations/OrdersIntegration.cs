using System;
using AspiringDemo.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.IO;
using System.Linq;
using AspiringDemo.Orders;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using AspiringDemo.Saving;
using AspiringDemo.Sites;
using AspiringDemo.Factions;

namespace AspiringDemoTest
{
    [TestClass]
    public class OrdersIntegration
    {
        Faction muldvarpFaction, bjorneFaction;
        ISquad bjorneSquad, muldvarpSquad;

        //TODO: Rewrite this so it isnt as much an integration test as an unit test

        public OrdersIntegration()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            TestSave save = new TestSave("Testo");
            GameFrame.Game.Savegame = save;
            GameFrame.Game.ObjectFactory = save;
            GameFrame.Game.ZonesHeight = 9;
            GameFrame.Game.ZonesWidth = 9;
            GameFrame.Game.Initialize();
            GameFrame.Game.ZonePathfinder.Nodes = Factories.GetZones(9, 9);

            bjorneFaction = GameFrame.Game.CreateFaction();
            muldvarpFaction = GameFrame.Game.CreateFaction();

            // moles
            muldvarpSquad = muldvarpFaction.CreateSquad();
            IUnit muldvarp1 = muldvarpFaction.CreateUnit();
            IUnit muldvarp2 = muldvarpFaction.CreateUnit();
            IUnit muldvarpen = muldvarpFaction.CreateUnit();

            muldvarpen.Name = "Muldvarpen";
            muldvarpen.Hp = 150;
            muldvarpSquad.AddMember(muldvarp1);
            muldvarp1.Name = "dooby";
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);
            muldvarpen.Rank = SquadRank.Commander;

            // bears
            IUnit lillebjorn = bjorneFaction.CreateUnit();
            lillebjorn.Name = "Lillebjorn";
            IUnit storebjorn = bjorneFaction.CreateUnit();
            storebjorn.Name = "Storebjorn";
            bjorneSquad = bjorneFaction.CreateSquad();

            bjorneSquad.AddMember(lillebjorn);
            bjorneSquad.AddMember(storebjorn);

            foreach (var unit in muldvarpFaction.Army.Units)
                GameFrame.Game.ZonePathfinder.Nodes[0].EnterZone(unit);


            foreach (var unit in bjorneFaction.Army.Units)
                GameFrame.Game.ZonePathfinder.Nodes[8].EnterZone(unit);
        }

        [TestMethod]
        public void TravelOrder()
        {
            GameFrame.Game.GameTime.TimeTicker -= bjorneFaction.GameTimeTick;

            IUnit muldvarpen = muldvarpSquad.Leader;
            GameFrame.Game.ZonePathfinder.Nodes.First().EnterZone(muldvarpen);

            
            var targetZone = GameFrame.Game.ZonePathfinder.Nodes[2];
            var travelPath = GameFrame.Game.ZonePathfinder.GetPath(muldvarpen.Zone, targetZone);

            TravelOrder order = new TravelOrder(muldvarpen, targetZone);
            Assert.AreEqual(targetZone, travelPath.Last());

            order = new TravelOrder(muldvarpen, targetZone);

            muldvarpen.AssignOrder(order);
            muldvarpen.Order.Execute();

            GameFrame.Game.GametimeTick();
            Assert.AreEqual(UnitState.ExecutingOrder, muldvarpen.State);

            //Assert.IsNotNull(GameFrame.Game.GameTime.TimeTicker);

            for (int i = 0; i < 30; i++)
            {
                GameFrame.Game.GametimeTick();

                if (muldvarpen.Order == null)
                    break;
            }

            Assert.IsTrue(GameFrame.Game.GameTime.Time > 2);
            //NB: unit being idle checks out because it is not added to Faction.Units.
            Assert.AreEqual(UnitState.Idle, muldvarpen.State);
            Assert.IsTrue(muldvarpen.Order == null);
            Assert.AreEqual(targetZone, muldvarpen.Zone);
        }

        //[TestMethod]
        //public void Attack_And_Raze_OutPost()
        //{
        //    throw new NotImplementedException("Need to rewrite or remove attack..");

        //    GameFrame.Game.Factions.ForEach(faction =>  faction.FactionManager.UnitManager = null);

        //    //TODO: Need to change followorder into travelorder 
        //    GameFrame.Game.GameTime.TimeTicker -= bjorneFaction.GameTimeTick;
        //    IUnit muldvarpen = muldvarpSquad.Leader;
        //    var startZone = GameFrame.Game.ZonePathfinder.Nodes.First();
        //    var targetZone = GameFrame.Game.ZonePathfinder.Nodes[16];


        //    startZone.EnterZone(muldvarpSquad);
        //    var bjorneFort = new Outpost(bjorneFaction, null);
        //    targetZone.EnterZone(bjorneSquad);
        //    targetZone.PopulatedAreas = new List<IPopulatedArea>();
        //    targetZone.PopulatedAreas.Add(bjorneFort);
            
        //    var order = new AttackArea(muldvarpen, bjorneFort);
        //    order.TargetZone = targetZone;
        //    order.TravelPath = GameFrame.Game.ZonePathfinder.GetPath(muldvarpen.Zone, order.TargetZone);

        //    muldvarpen.AssignOrder(order);
            

        //    order.Execute();

        //    Assert.AreEqual(UnitState.ExecutingOrder, muldvarpen.State);
        //    Assert.AreEqual(2, muldvarpSquad.Members.Where(x => x != muldvarpen && x.Order != null).Count());
        //    Assert.AreEqual(muldvarpen, ((FollowOrder)(muldvarpSquad.Members.FirstOrDefault(x => x != muldvarpen && x.Order != null).Order)).FollowTarget);

        //    for (int i = 0; i < 18; i++)
        //    {
        //        GameFrame.Game.GametimeTick();

        //        if (targetZone.Fight != null)
        //            targetZone.Fight.PerformFightRound();
        //    }


        //    Assert.AreEqual(order.TargetZone, muldvarpen.Zone);
        //    Assert.AreEqual(UnitState.Idle, muldvarpen.State);
        //    Assert.AreEqual(true, bjorneFort.Razed);
        //}

        public List<IZone> GetTestZones()
        {
            IZone zone1 = new Zone();
            zone1.PositionXStart = 0;
            zone1.PositionXEnd = 500;
            zone1.PositionYStart = 0;
            zone1.PositionYEnd = 500;

            IZone zone2 = new Zone();
            zone2.PositionXStart = 501;
            zone2.PositionXEnd = 1000;
            zone2.PositionYStart = 0;
            zone2.PositionYEnd = 500;

            IZone zone3 = new Zone();
            zone3.PositionXStart = 1001;
            zone3.PositionXEnd = 1501;
            zone3.PositionYStart = 0;
            zone3.PositionYEnd = 500;

            IZone fuckzone = new Zone();
            fuckzone.PositionXStart = 501;
            fuckzone.PositionXEnd = 1000;
            fuckzone.PositionYStart = 501;
            fuckzone.PositionYEnd = 1000;


            zone1.AddNeighbour(zone2);
            zone2.AddNeighbour(zone1);
            zone2.AddNeighbour(zone3);
            zone2.AddNeighbour(fuckzone);
            zone3.AddNeighbour(zone2);
            fuckzone.AddNeighbour(zone2);

            List<IZone> zones = new List<IZone>();
            zones.Add(zone1);
            zones.Add(zone2);
            zones.Add(zone3);
            zones.Add(fuckzone);

            return zones;
        }
    }
}
