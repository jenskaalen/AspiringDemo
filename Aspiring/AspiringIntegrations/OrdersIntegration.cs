using System;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;
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
            IUnit muldvarp1 = muldvarpFaction.Create<Unit>();
            IUnit muldvarp2 = muldvarpFaction.Create<Unit>();
            IUnit muldvarpen = muldvarpFaction.Create<Unit>();

            muldvarpen.Name = "Muldvarpen";
            muldvarpen.Hp = 150;
            muldvarpSquad.AddMember(muldvarp1);
            muldvarp1.Name = "dooby";
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);
            muldvarpen.Rank = SquadRank.Commander;

            // bears
            IUnit lillebjorn = bjorneFaction.Create<Unit>();
            lillebjorn.Name = "Lillebjorn";
            IUnit storebjorn = bjorneFaction.Create<Unit>();
            storebjorn.Name = "Storebjorn";
            bjorneSquad = bjorneFaction.CreateSquad();

            bjorneSquad.AddMember(lillebjorn);
            bjorneSquad.AddMember(storebjorn);

            foreach (var unit in muldvarpFaction.Army.Units)
                unit.EnterZone(GameFrame.Game.ZonePathfinder.Nodes[0]);


            foreach (var unit in bjorneFaction.Army.Units)
                unit.EnterZone(GameFrame.Game.ZonePathfinder.Nodes[8]);
        }

        [TestMethod]
        public void Perform_Travel_Order()
        {
            GameFrame.Game.GameTime.TimeTicker -= bjorneFaction.GameTimeTick;

            IUnit muldvarpen = muldvarpSquad.Leader;

            muldvarpen.EnterZone(GameFrame.Game.ZonePathfinder.Nodes.First());
            
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
            IZone zone1 = new Zone(0, 0, 500, 500);
            IZone zone2 = new Zone(501, 0, 500, 500);
            IZone zone3 = new Zone(1001, 0, 500, 500);
            IZone fuckzone = new Zone(501, 501, 500, 500);

            //IZone zone2 = new Zone();
            //zone2.Area.X1 = 501;
            //zone2.X1 = 1000;
            //zone2.Area.Y1 = 0;
            //zone2.PositionYEnd = 500;

            //IZone zone3 = new Zone();
            //zone3.Area.X1 = 1001;
            //zone3.X1 = 1501;
            //zone3.Area.Y1 = 0;
            //zone3.PositionYEnd = 500;

            //IZone fuckzone = new Zone();
            //fuckzone.Area.X1 = 501;
            //fuckzone.X1 = 1000;
            //fuckzone.Area.Y1 = 501;
            //fuckzone.PositionYEnd = 1000;


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
