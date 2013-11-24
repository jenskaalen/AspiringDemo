using System;
using AspiringDemo.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.Linq;
using System.Data;
using System.IO;
using System.Collections.Generic;
using AspiringDemo.Saving;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using Ninject;

namespace AspiringDemoTest
{
    //TODO: Refactor name
    [TestClass]
    public class Unsorted
    {
        private string testdb = "test1";

        [TestInitialize]
        public void Initialize()
        {
            TestSave savegame = new TestSave(testdb);
            GameFrame.Game.Savegame = savegame;
            GameFrame.Game.ObjectFactory = (IObjectFactory)GameFrame.Game.Savegame;

            GameFrame.Game.ZonesHeight = 3;
            GameFrame.Game.ZonesWidth = 3;
            GameFrame.Game.Initialize();
            IFaction faction1 = GameFrame.Game.CreateFaction();
            faction1.Name = "Bontierne";
            IFaction faction2 = GameFrame.Game.CreateFaction();
            faction2.Name = "Muldvarp-battaljonen";

            ISquad muldvarpSquad = faction2.CreateSquad();
            IUnit muldvarp1 = faction2.CreateUnit();
            IUnit muldvarp2 = faction2.CreateUnit();
            IUnit muldvarpen = faction2.CreateUnit();
            muldvarpen.Name = "Muldvarpen";
            muldvarpSquad.AddMember(muldvarp1);
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);

            IUnit lillebjorn = faction1.CreateUnit();
            IUnit storebjorn = faction1.CreateUnit();

            ISquad bjornesquad = faction1.CreateSquad();
            bjornesquad.AddMember(lillebjorn);
            bjornesquad.AddMember(storebjorn);
        }

        [TestMethod]
        public void ChangeRank()
        {
            ISquad muldvarpsquad = GameFrame.Game.Factions.Where(x => x.Name.Contains("Muldvarp")).FirstOrDefault().Army.Squads.FirstOrDefault();
            IUnit muldvarpen = muldvarpsquad.Members.Where(x => x.Name == "Muldvarpen").FirstOrDefault();
            muldvarpen.Rank = SquadRank.Commander;
            Assert.AreEqual(muldvarpen, muldvarpsquad.Leader);
        }

        [TestMethod]
        public void CompleteFight()
        {
            ISquad squad1 = GameFrame.Game.Factions[0].Army.Squads.FirstOrDefault();
            ISquad squad2 = GameFrame.Game.Factions[1].Army.Squads.FirstOrDefault();

            GameFrame.Game.Pathfinding.Zones[2].EnterZone(squad1);
            GameFrame.Game.Pathfinding.Zones[2].EnterZone(squad2);

            IZone testzone = GameFrame.Game.Pathfinding.Zones[2];

            Assert.IsTrue(testzone.Fight != null);
            Assert.IsTrue(testzone.Fight.FightersCount == 5);

            Assert.IsNotNull(testzone.Fight);

            for (int i = 0; i < 10; i++)
            {
                if (testzone.Fight != null)
                    testzone.Fight.PerformFightRound();
            }

            Assert.IsNull(testzone.Fight);
        }

        [TestMethod]
        public void IsolatedFight()
        {
            IZone zonudes = new Zone();
            IFaction f1 = GameFrame.Game.Factory.Get<IFaction>();
            IFaction f2 = GameFrame.Game.Factory.Get<IFaction>();

            ISquad squad1 = f1.CreateSquad();

            var unit1 = f1.CreateUnit();
            var unit2 = f2.CreateUnit();
            var unit3 = f1.CreateUnit();

            squad1.AddMember(unit1);
            squad1.AddMember(unit3);

            unit3.Rank = SquadRank.Veteran;

            Assert.IsTrue(squad1.Leader == unit3);

            zonudes.EnterZone(unit1);
            zonudes.EnterZone(unit3);
            zonudes.EnterZone(unit2);

            Assert.IsTrue(zonudes.Fight != null);

            while(zonudes.Fight != null)
                zonudes.Fight.PerformFightRound();

            Assert.IsTrue(zonudes.Fight == null);
            Assert.AreEqual(UnitState.Dead, unit2.State);
        }


        [TestMethod]
        public void ComputedPathfinding()
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

            GameFrame.Game.Pathfinding.Zones = new List<IZone>();
            GameFrame.Game.Pathfinding.Zones.Add(zone1);
            GameFrame.Game.Pathfinding.Zones.Add(fuckzone);
            GameFrame.Game.Pathfinding.Zones.Add(zone2);
            GameFrame.Game.Pathfinding.Zones.Add(zone3);

            List<IZone> path = GameFrame.Game.ZonePathfinder.GetPath(zone1, zone3);

            Assert.AreEqual(2, path.Count);
        }

        [TestMethod]
        public void PlayerEnterLeaveArea()
        {
            Unit player = new Unit(new Faction());
            player.IsPlayer = true;
            IZone zone = new Zone();
            zone.EnterZone(player);

            Assert.IsTrue(zone.IsPlayerNearby == true);

            IZone zone2 = new Zone();
            zone2.EnterZone(player);

            Assert.IsTrue(zone.IsPlayerNearby == false);

            Squad s1 = new Squad();
            s1.AddMember(player);
            zone.EnterZone(s1);

            Assert.IsTrue(zone.IsPlayerNearby == true);
            zone2.EnterZone(s1);
            Assert.IsTrue(zone.IsPlayerNearby == false);
        }
    }
}
