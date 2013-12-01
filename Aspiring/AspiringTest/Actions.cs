using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
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
        public void GameAction_AttackUnit()
        {
            var faction1 = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();
            var unit1 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction1));
            unit1.Items.Weapons.Add(new Sword());
            var unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));
            var zone = new Zone(0, 0, 499, 499);
            unit1.EnterZone(zone);
            unit2.EnterZone(zone);

            var attack = new UnitAttack(unit1, unit2);
            attack.Update(1f);
            Assert.AreEqual(UnitState.Fighting, unit2.State);

            for (int i=0; i < 15; i++)
                attack.Update(i);

            Assert.AreEqual(UnitState.Dead, unit2.State);
        }
    }
}
