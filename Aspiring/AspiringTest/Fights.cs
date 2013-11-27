using System;
using AspiringDemo.Combat;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Gamecore;
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
        public void Unit_Selects_Best_Weapon()
        {
            var faction = Factories.Kernel.Get<IFaction>();
            var unit = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction));
            IWeapon wpn = new Sword();
            IWeapon unarmed = new Sword();
            unit.Items.Weapons.Add(wpn);
            unit.Items.Weapons.Add(unarmed);

            var bestWpn = unit.Items.GetBestWeapon();
            Assert.AreEqual(wpn, bestWpn);
        }

        [TestMethod]
        public void Fight_Ends()
        {
            var faction1 = Factories.Kernel.Get<IFaction>();
            var faction2 = Factories.Kernel.Get<IFaction>();

            var unit1 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction1));
            var unit2 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction2));

            unit1.Hp = 20;

            INewFight fight = GameFrame.Game.Factory.Get<INewFight>();
            fight.Enter(unit1);
            fight.Enter(unit2);

            unit1.Items.Weapons.Add(new Smackhammer());
            unit2.Items.Weapons.Add(new Smackhammer());

            GameFrame.Game.ActionProcesser.Update(1f);
            GameFrame.Game.ActionProcesser.Update(2f);
            GameFrame.Game.ActionProcesser.Update(3f);

            Assert.AreEqual(false, fight.Finished);
        }

        [TestMethod]
        public void Fight_Starts_When_Enemies_Enter_Same_Zone()
        {
            IZone fightZone = new Zone();
            var f1 = GameFrame.Game.Factory.Get<IFaction>();
            var f2 = GameFrame.Game.Factory.Get<IFaction>();

            var unit1 = f1.CreateUnit();
            var unit2 = f1.CreateUnit();
            var unit3 = f2.CreateUnit();

            unit1.EnterZone(fightZone);
            unit2.EnterZone(fightZone);
            unit3.EnterZone(fightZone);

            Assert.IsTrue(unit1.CombatModule.CurrentFight != null);
            Assert.IsTrue(unit2.CombatModule.CurrentFight != null);

            for (int i = 0; i < 20; i++)
                GameFrame.Game.ActionProcesser.Update(i);

            Assert.IsTrue(unit1.CombatModule.CurrentFight == null);
            Assert.AreEqual(UnitState.Dead, unit3.State);
        }


        [TestMethod]
        public void Unit_Get_Potential_Targets()
        {
            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            // ^ALLIES
            var faction3 = GameFrame.Game.Factory.Get<IFaction>();

            var unit1 = faction1.CreateUnit();
            var unit2 = faction2.CreateUnit();
            var unit3 = faction3.CreateUnit();

            faction1.Relations.SetRelation(faction2, RelationType.Friendly);
            faction2.Relations.SetRelation(faction1, RelationType.Friendly);

            var allUnits = new List<IUnit>()
            {
                unit1,
                unit2,
                unit3
            };

            Assert.AreEqual(1, unit1.CombatModule.GetPotentialTargets(allUnits).Count);
        }
    }
}
