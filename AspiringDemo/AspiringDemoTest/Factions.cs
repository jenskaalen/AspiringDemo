using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using AspiringDemo;
using AspiringDemo.ANN;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Sites;
using AspiringDemo.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Factions
    {
        [TestMethod]
        public void Measure_Faction_Strength_Rating_Values()
        {
            StrengthMap sm = new StrengthMap();
            var strAbysmal = 0.2;
            var strWeak = 0.5;
            var strMedium = 0.9;
            var strStrong = 1.5;
            var strHulk = 2.2;

            Assert.AreEqual(StrengthMeasurement.Abysmal, sm.GetStrengthMeasurement(strAbysmal));
            Assert.AreEqual(StrengthMeasurement.Weak, sm.GetStrengthMeasurement(strWeak));
            Assert.AreEqual(StrengthMeasurement.Medium, sm.GetStrengthMeasurement(strMedium));
            Assert.AreEqual(StrengthMeasurement.Strong, sm.GetStrengthMeasurement(strStrong));
            Assert.AreEqual(StrengthMeasurement.Hulk, sm.GetStrengthMeasurement(strHulk));
        }

        [TestMethod]
        public void Factions_Strength_Calculation()
        {
            Game game = new Game();

            IFaction faction1 = new Faction();
            faction1.Initialize();
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));
            faction1.Army.Units.Add(new Unit(faction1));

            IFaction faction2 = new Faction();
            faction2.Initialize();
            faction2.Army.Units.Add(new Unit(faction2));

            IFaction faction3 = new Faction();
            faction3.Initialize();
            faction3.Army.Units.Add(new Unit(faction3));

            var factions = new List<IFaction>();
            factions.Add(faction1);
            factions.Add(faction2);
            factions.Add(faction3);

            var sm = new StrengthMap();

            sm.MapFactions(factions);

            Assert.AreEqual(StrengthMeasurement.Hulk, faction1.Strength);
            Assert.AreEqual(StrengthMeasurement.Abysmal, faction2.Strength);
        }

        [TestMethod]
        public void Test_Faction_Reference_Stays_Updated()
        {
            Faction faction = new Faction();
            faction.FactionManager = new FactionManager(faction);

            Assert.AreEqual(faction.FactionManager.UnitManager.Faction.Areas.Count, faction.Areas.Count);

            faction.AddArea(new PopulatedArea(faction, null));

            Assert.AreEqual(faction.FactionManager.UnitManager.Faction.Areas.Count, faction.Areas.Count);
        }

        [TestMethod]
        public void Unitmanager_Form_Squad()
        {
            GameFrame.Game.Factions = new List<IFaction>();
            GameFrame.Game.CreateFaction();

            var faction = GameFrame.Game.Factions[0];

            faction.CreateUnit();
            faction.CreateUnit();

            faction.FactionManager.UnitManager.ManageUnits();
            Assert.AreEqual(0, faction.Army.Squads.Count);


            faction.CreateUnit();
            faction.FactionManager.UnitManager.ManageUnits();

            Assert.AreEqual(1, faction.Army.Squads.Count);
            Assert.AreEqual(3, faction.Army.Units.Count(unit => unit.Squad != null));

        }

        [TestMethod]
        public void Set_Diplomatic_Relation()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();

            var relations = GameFrame.Game.Factory.Get<IFactionRelations>();
            relations.SetRelation(faction, RelationType.Friendly);

            Assert.AreEqual(RelationType.Friendly, relations.GetRelation(faction).Relation);

            relations.SetRelation(faction, RelationType.Hostile);

            
        }

        [TestMethod]
        public void Get_Standard_Diplomatic_Relation()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var relations = GameFrame.Game.Factory.Get<IFactionRelations>();

            Assert.AreEqual(RelationType.Hostile, relations.GetRelation(faction).Relation);
        }


        [TestMethod]
        public void Check_Hostility_Among_Several_Factions()
        {
            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            var faction3 = GameFrame.Game.Factory.Get<IFaction>();

            faction1.Relations.SetRelation(faction2, RelationType.Friendly);
            faction2.Relations.SetRelation(faction1, RelationType.Friendly);

            var list = new List<IFaction> {faction1, faction2, faction3};

            bool containsHostile = FactionRelations.ContainsHostileFactions(list);
            Assert.IsTrue(containsHostile);

            faction3.Relations.SetRelation(faction1, RelationType.Friendly);
            faction3.Relations.SetRelation(faction2, RelationType.Friendly);

            faction1.Relations.SetRelation(faction3, RelationType.Friendly);
            faction2.Relations.SetRelation(faction3, RelationType.Friendly);

            containsHostile = FactionRelations.ContainsHostileFactions(list);
            Assert.IsFalse(containsHostile);
        }
    }
}
