using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.IO;

namespace AspiringDemoTest
{
    [TestClass]
    public class UnitTest1
    {


        [TestInitialize]
        public void Initialize()
        {
            Game.ZonesHeight = 3;
            Game.ZonesWidth = 3;
            Game.Initialize();
            Faction faction1 = new Faction(false, "Raggarbjornanen");
            Faction faction2 = new Faction(false, "Muldvarp-bataljonen");
            Game.AddFaction(faction1);
            Game.AddFaction(faction2);

            Squad muldvarpSquad = faction2.CreateSquad();
            Unit muldvarp1 = new Unit();
            Unit muldvarp2 = new Unit();
            Unit muldvarpen = new Unit();
            muldvarpen.Damage = 50;
            muldvarpen.Name = "Muldvarpen";
            muldvarpSquad.AddMember(muldvarp1);
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);

            Unit lillebjorn = new Unit();
            Unit storebjorn = new Unit();
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

        [TestMethod]
        public void TestZoneFinding()
        {
            Zone zone1 = new Zone();
            zone1.PositionXStart = 0;
            zone1.PositionXEnd = 500;
            zone1.PositionYStart = 0;
            zone1.PositionYEnd = 500;

            Zone zone2 = new Zone();
            zone2.PositionXStart = 501;
            zone2.PositionXEnd = 1000;
            zone2.PositionYStart = 501;
            zone2.PositionYEnd = 1000;

            int testposX = 570;
            int testposY = 570;

            Pathfinding path = new Pathfinding();
            path.Zones = new System.Collections.Generic.List<Zone>();
            path.Zones.Add(zone1);
            path.Zones.Add(zone2);

            var gottenzone = path.GetZone(testposX, testposY);

            Assert.AreEqual(zone2, gottenzone);
        }


        [TestMethod]
        public void ZoneCreation()
        {
            Assert.AreEqual(9, Game.Pathfinding.Zones.Count);
        }

        [TestMethod]
        public void TestCompleteFight()
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
        public void ZonedPathfinding()
        { 
            
        }

        [TestMethod]
        public void Looting()
        {

        }

        [TestMethod]
        public void CreateSaveLoadDatabaseGame()
        {
            Weapon w1 = new Weapon();
            w1.WeaponName = "lala";
            w1.BaseDamage = 5;
            //Weapon w2 = new Weapon();
            //w2.WeaponName = "fuufu";

            Game.Weapons = new System.Collections.Generic.List<Weapon>();
            Game.Weapons.Add(w1);

            Assert.AreEqual(5, Game.Weapons[0].BaseDamage);

            if (File.Exists("butterflies.sdf"))
                File.Delete("butterflies.sdf");
            
            SaveGame savegame = new SaveGame("butterflies");
            Game.SaveGame = savegame;

            Assert.AreEqual(5, Game.Weapons[0].BaseDamage);


            savegame.Save();
            savegame.Load();
            Assert.AreEqual(5, Game.Weapons[0].BaseDamage);

            w1.BaseDamage = 10;
            savegame.Save();
            savegame.Load();
            Assert.AreEqual(10, Game.Weapons[0].BaseDamage);

        }
    }
}
