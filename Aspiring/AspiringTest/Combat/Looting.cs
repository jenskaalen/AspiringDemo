using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Units;
using AspiringDemo.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest.Combat
{
    [TestClass]
    public class Looting
    {
        [TestMethod]
        public void Loot()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));
            var unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));

            var smack = new Smackhammer();
            unit2.Items.Weapons.Add(smack);

            unit.CombatModule.AttackTarget(unit2);

            Assert.IsTrue(unit.Items.Weapons.Contains(smack));
            Assert.IsTrue(!unit2.Items.Weapons.Contains(smack));
        }
    }
}
