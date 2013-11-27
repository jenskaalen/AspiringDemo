using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Units;
using AspiringDemo.Units.Actions;
using AspiringDemo.Weapons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using AspiringDemo.ANN.Actions.Unit;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Actions
    {
        [TestMethod]
        public void AttackUnit_Unit()
        {
            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            var unit1 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction1));
            unit1.Items.Weapons.Add(new Sword());
            var unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));

            var attack = new UnitAttack(unit1, unit2);
            attack.Update(1f);
            Assert.AreEqual(UnitState.Idle, unit2.State);

            for (int i=0; i < 15; i++)
                attack.Update(1);

            Assert.AreEqual(UnitState.Dead, unit2.State);
        }
    }
}
