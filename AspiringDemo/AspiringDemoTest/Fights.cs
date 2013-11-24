using System;
using AspiringDemo.Combat;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using Ninject;
using System.Collections.Generic;
using AspiringDemo.Factions;
using AspiringDemo.Weapons;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Fights
    {
        delegate void TestDelegate();
        
        [TestMethod]
        public void Unit_Selects_Weapon()
        {
            var unit = Factories.Kernel.Get<IUnit>();
            IWeapon wpn = new Sword();

            unit.Weapons = new List<IWeapon>();
            unit.Weapons.Add(wpn);

            var weapon = unit.SelectBestWeapon();
            Assert.IsTrue(weapon == wpn);
        }

        [TestMethod]
        public void Fight_Ends()
        {
            var faction1 = Factories.Kernel.Get<IFaction>();
            var faction2 = Factories.Kernel.Get<IFaction>();

            var unit1 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction1));
            var unit2 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction2));

            unit1.Hp = 20;

            Fight fight = new Fight();
            fight.AddUnit(unit1);
            fight.AddUnit(unit2);

            unit1.Weapons.Add(new Smackhammer());
            unit2.Weapons.Add(new Smackhammer());

            //unit1.ChangeState(unit1, UnitState.Dead);
            fight.PerformFightRound();
            fight.PerformFightRound();

            Assert.AreEqual(false, fight.FightActive);
        }

        [TestMethod]
        public void Wont_Attack_Allies()
        {
            Fight fight = new Fight();

            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            // ^ALLIES
            var faction3 = GameFrame.Game.Factory.Get<IFaction>();

            var unit1 = faction1.CreateUnit();
            var unit2 = faction2.CreateUnit();
            var unit3 = faction3.CreateUnit();

            fight.AddUnit(unit1);
            fight.AddUnit(unit2);
            fight.AddUnit(unit3);

            faction1.Relations.SetRelation(faction2, RelationType.Friendly);
            faction2.Relations.SetRelation(faction1, RelationType.Friendly);

            Assert.AreEqual(1, fight.GetViableTargets(unit1).Count);
        }
    }
}
