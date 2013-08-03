using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Collections.Generic;

namespace AspiringDemoTest
{
    [TestClass]
    public class UnitTest1
    {

        public UnitTest1()
        {
            if (File.Exists("butterflies.sdf"))
                File.Delete("butterflies.sdf");

            SaveGame savegame = new SaveGame("butterflies");
            Game.SaveGame = savegame;
            Game.ObjectFactory = (IObjectFactory) Game.SaveGame;

            Game.ZonesHeight = 3;
            Game.ZonesWidth = 3;
            Game.Initialize();
            Faction faction1 = new Faction(false, "Raggarbjornanen");
            Faction faction2 = new Faction(false, "Muldvarp-bataljonen");
            Game.AddFaction(faction1);
            Game.AddFaction(faction2);

            Squad muldvarpSquad = faction2.CreateSquad();
            Unit muldvarp1 = Game.ObjectFactory.GetObject<Unit>();
            Unit muldvarp2 = Game.ObjectFactory.GetObject<Unit>();
            Unit muldvarpen = Game.ObjectFactory.GetObject<Unit>();
            muldvarpen.Damage = 50;
            muldvarpen.Name = "Muldvarpen";
            muldvarpSquad.AddMember(muldvarp1);
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);

            Unit lillebjorn = Game.ObjectFactory.GetObject<Unit>();
            Unit storebjorn = Game.ObjectFactory.GetObject<Unit>();
            Squad bjornesquad = faction1.CreateSquad();
            bjornesquad.AddMember(lillebjorn);
            bjornesquad.AddMember(storebjorn);
        }

        [TestMethod]
        public void ChangeRank()
        {
            Squad muldvarpsquad = Game.Factions.Where(x => x.ID.Contains("Muldvarp")).FirstOrDefault().Squads.FirstOrDefault();
            Unit muldvarpen = muldvarpsquad.Members.Where(x => x.Name == "Muldvarpen").FirstOrDefault();
            muldvarpen.Rank = SquadRank.Commander;
            Assert.AreEqual(muldvarpen, muldvarpsquad.Leader);
        }

        //TODO: Remove
        //[TestMethod]
        //public void TestZoneFinding()
        //{
        //    Zone zone1 = Game.ObjectFactory.GetObject<Zone>();
        //    zone1.PositionXStart = 0;
        //    zone1.PositionXEnd = 500;
        //    zone1.PositionYStart = 0;
        //    zone1.PositionYEnd = 500;

        //    Zone zone2 = Game.ObjectFactory.GetObject<Zone>();
        //    zone2.PositionXStart = 501;
        //    zone2.PositionXEnd = 1000;
        //    zone2.PositionYStart = 501;
        //    zone2.PositionYEnd = 1000;

        //    int testposX = 570;
        //    int testposY = 570;

        //    Pathing path = new Pathing();
        //    path.Zones = new System.Collections.Generic.List<Zone>();
        //    path.Zones.Add(zone1);
        //    path.Zones.Add(zone2);

        //    var gottenzone = path.GetZone(testposX, testposY);

        //    Assert.AreEqual(zone2, gottenzone);
        //}


        [TestMethod]
        public void ZoneCreation()
        {
            //Assert.AreEqual(9, Game.Pathfinding.Zones.Count);
        }

        [TestMethod]
        public void CompleteFight()
        {
            Squad squad1 = Game.Factions[0].Squads.FirstOrDefault();
            Squad squad2 = Game.Factions[1].Squads.FirstOrDefault();

            squad1.Zone = Game.Pathfinding.Zones[2];
            squad2.Zone = Game.Pathfinding.Zones[2];

            Zone testzone = squad1.Zone;

            Assert.IsTrue(Game.IsZoneContested(testzone));

            var squads = Game.IsZoneContestedSquads(testzone);

            Assert.AreEqual(2, squads.Count);

            Game.ProcessZone(testzone);

            Assert.AreEqual(SquadState.Fighting, squads[0].State);
            Assert.IsNotNull(testzone.Fight);

            for (int i = 0; i < 10; i++)
            {
                Game.ProcessZone(testzone);
            }

            Assert.IsNull(testzone.Fight);
        }

        [TestMethod]
        public void ComputedPathfinding()
        {
            Zone zone1 = new Zone(); 
            zone1.PositionXStart = 0;
            zone1.PositionXEnd = 500;
            zone1.PositionYStart = 0;
            zone1.PositionYEnd = 500;

            Zone zone2 = new Zone();
            zone2.PositionXStart = 501;
            zone2.PositionXEnd = 1000;
            zone2.PositionYStart = 0;
            zone2.PositionYEnd = 500;

            Zone zone3 = new Zone();
            zone3.PositionXStart = 1001;
            zone3.PositionXEnd = 1501;
            zone3.PositionYStart = 0;
            zone3.PositionYEnd = 500;

            Zone fuckzone = new Zone();
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

            Game.Pathfinding.Zones = new List<Zone>();
            Game.Pathfinding.Zones.Add(zone1);
            Game.Pathfinding.Zones.Add(fuckzone);
            Game.Pathfinding.Zones.Add(zone2);
            Game.Pathfinding.Zones.Add(zone3);

            List<Zone> path = Game.ZonePathfinder.GetPath(zone1, zone3);

            Assert.AreEqual(2, path.Count);
        }

        [TestMethod]
        public void Looting()
        {

        }

        [TestMethod]
        public void CreateSaveLoadDatabaseGame()
        {
            Weapon w1 = Game.ObjectFactory.GetObject<Weapon>();
            w1.WeaponName = "lala";
            w1.BaseDamage = 5;

            Game.Weapons = new System.Collections.Generic.List<Weapon>();
            Game.Weapons.Add(w1);

            Assert.AreEqual(5, Game.Weapons[0].BaseDamage);

            Game.SaveGame.Save();
            Game.SaveGame.Load();
            Assert.AreEqual(5, Game.Weapons[0].BaseDamage);

            w1.BaseDamage = 10;
            Game.SaveGame.Save();
            Game.SaveGame.Load();
            Assert.AreEqual(10, Game.Weapons[0].BaseDamage);

        }
    }
}
