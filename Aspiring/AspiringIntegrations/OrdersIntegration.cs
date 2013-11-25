using System;
using AspiringDemo.Gamecore;
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

        public OrdersIntegration()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            GameFrame.SetGame(new Game());
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
        public void Perform_Travel_Order()
        {
            GameFrame.Game.GameTime.TimeTicker -= bjorneFaction.GameTimeTick;

            IUnit muldvarpen = muldvarpSquad.Leader;

            GameFrame.Game.ZonePathfinder.Nodes.First().EnterZone(muldvarpen);
            
            var targetZone = GameFrame.Game.ZonePathfinder.Nodes[2];

            TravelOrder order = new TravelOrder(muldvarpen, targetZone);
            TravelOrder.GiveTravelOrder(muldvarpen, targetZone, false);
            Assert.IsTrue(muldvarpen.Order != null);
            muldvarpen.Order.Execute();

            GameFrame.Game.GametimeTick();
            Assert.AreEqual(UnitState.ExecutingOrder, muldvarpen.State);


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
