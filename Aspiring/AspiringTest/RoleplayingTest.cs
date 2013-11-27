using System;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Units;
using AspiringDemo.Units.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using AspiringDemo.Roleplaying;
using AspiringDemo.Factions;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class RoleplayingTest
    {
        [TestMethod]
        public void KillGainXPAndLevel()
        {
            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            //var unit1 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction1));
            var unit1 = new Unit(faction1);
            var unit2 = new Unit(faction2);

            var attack = new UnitAttack(unit1, unit2);

            for (int i = 1; i < 30; i++)
                attack.Update(i);

            Assert.IsTrue(unit2.State == UnitState.Dead);
            Assert.AreEqual(1, unit1.CombatModule.Kills);
            Assert.IsTrue(unit1.CharacterLevel.CurrentXP > 0);
            Assert.AreEqual(1, unit1.CharacterLevel.Level);
        }

        [TestMethod]
        public void Gain_Huge_Xp()
        {
            var unit = new Unit(new Faction());
            unit.CharacterLevel.GainXP(5000);

            Assert.IsTrue(unit.CharacterLevel.Level > 1);
        }

        [TestMethod]
        public void Gain_Xp_And_Level_Up()
        {
            var unit1 = new Unit(new Faction())
            {
                CharacterLevel =
                    new CharacterLevel(new LevelProgressModifier() {XpMultiplier = 2.0}) {Level = 1, NextLevelXP = 100}
            };

            unit1.CharacterLevel.GainXP(150);

            Assert.AreEqual(2, unit1.CharacterLevel.Level);
            Assert.AreEqual(200, unit1.CharacterLevel.NextLevelXP);
        }

        [TestMethod]
        public void Stats_Are_Increased_Upon_Leveling()
        {
            var unit = new Unit(new Faction())
            {
                Stats =
                {
                    BaseStrength = 10,
                    BaseSpeed = 10,
                    Strength = 10,
                    Speed = 10,
                    GrowthStrength = 10,
                    GrowthSpeed = 20
                }
            };

            Assert.AreEqual(10, unit.Stats.Strength);
            Assert.AreEqual(10, unit.Stats.Speed);
            unit.Stats.GainLevel();
            Assert.AreEqual(20, unit.Stats.Strength);
            Assert.AreEqual(30, unit.Stats.Speed);
        }

        [TestMethod]
        public void Regen_Hp()
        {
            var stats = new UnitStats {MaxHp = 50, BaseHp = 50, CurrentHp = 30, RegenRate = 10, RegenHpAmount = 5};

            stats.Regen(0);
            Assert.AreEqual(30, stats.CurrentHp);
            stats.Regen(15);
            Assert.AreEqual(35, stats.CurrentHp);
            stats.Regen(15);
            Assert.AreEqual(35, stats.CurrentHp);
        }
    }
}
