using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameActions;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
using AspiringDemo.Zones.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using AspiringDemo.ANN.Actions.Unit;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Actions
    {
        private IUnit unit1, unit2;
        private IFaction faction1, faction2;
        private List<InteriorNode> _nodes;
        private IZone somezone;

        [TestInitialize]
        public void Init()
        {
            faction1 = new Faction();
            faction2 = new Faction();
            unit1 = new Unit(faction1);
            unit2 = new Unit(faction2);
            somezone = new Zone(0, 0, 20, 20);
            somezone.Type = ZoneType.Interior;
            _nodes = TestUtil.GetTestNodes<InteriorNode>(20, 1);
        }

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

            for (int i = 0; i < 15; i++)
                attack.Update(i);

            Assert.AreEqual(UnitState.Dead, unit2.State);
        }

        //[TestMethod]
        //public void GameAction_DetectEnemies()
        //{
        //    unit1.EnterZone(somezone);
        //    unit2.EnterZone(somezone);

        //    bool isDetected = unit1.CombatModule.DetectEnemy(unit2);
        //    Assert.IsTrue(isDetected);

        //    var detect = new DetectEnemies(unit1);
        //    unit1.Actions.Add(detect);

        //    Assert.IsTrue(unit1.State == UnitState.Idle);
        //    Assert.AreEqual(1, unit1.Actions.Count);
            
        //    detect.Update(1f);
        //    Assert.AreEqual(2, unit1.Actions.Count);
        //}
    }
}
