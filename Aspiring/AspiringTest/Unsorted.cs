using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.Linq;
using System.Collections.Generic;
using AspiringDemo.Saving;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;

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
            var firstOrDefault = GameFrame.Game.Factions.FirstOrDefault(x => x.Name.Contains("Muldvarp"));
            if (firstOrDefault != null)
            {
                ISquad muldvarpsquad = firstOrDefault.Army.Squads.FirstOrDefault();
                IUnit muldvarpen = muldvarpsquad.Members.FirstOrDefault(x => x.Name == "Muldvarpen");
                muldvarpen.Rank = SquadRank.Commander;
                Assert.AreEqual(muldvarpen, muldvarpsquad.Leader);
            }
        }

        [TestMethod]
        public void CompleteFight()
        {
            ISquad squad1 = GameFrame.Game.Factions[0].Army.Squads.FirstOrDefault();
            ISquad squad2 = GameFrame.Game.Factions[1].Army.Squads.FirstOrDefault();

            squad1.EnterZone(GameFrame.Game.Pathfinding.Zones[2]);
            squad2.EnterZone(GameFrame.Game.Pathfinding.Zones[2]);

            IZone testzone = GameFrame.Game.Pathfinding.Zones[2];

            var fight = squad1.Members[0].CombatModule.CurrentFight;

            Assert.IsTrue(fight != null);
            Assert.IsTrue(fight.Units.Count == 5);

            var fighting = (Fighting) GameFrame.Game.ActionProcesser.Actions[0];

            for (int i = 0; i < 20; i++)
            {
                GameFrame.Game.ActionProcesser.Update(i);
            }

            Assert.IsTrue(GameFrame.Game.ActionProcesser.Actions.Count == 0);
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

        public void PlayerEnterLeaveArea()
        {
            Unit player = new Unit(new Faction());
            player.IsPlayer = true;
            IZone zone = new Zone();
            player.EnterZone(zone);
            Assert.IsTrue(zone.IsPlayerNearby);

            IZone zone2 = new Zone();
            player.EnterZone(zone2);
            Assert.IsTrue(zone.IsPlayerNearby == false);

            Squad s1 = new Squad();
            s1.AddMember(player);
            s1.EnterZone(zone);
            Assert.IsTrue(zone.IsPlayerNearby);

            s1.EnterZone(zone2);
            Assert.IsTrue(zone.IsPlayerNearby == false);
        }
    }
}
