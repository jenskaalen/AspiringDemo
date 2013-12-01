using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Procedural;
using AspiringDemo.Zones.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest.Interior
{
    [TestClass]
    public class Interiors
    {
        private IUnit _unit;
        private IFaction _faction;

        [TestInitialize]
        public void Init()
        {
            _faction = GameFrame.Game.Factory.Get<IFaction>();
            _unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", _faction));
        }

        [TestMethod]
        public void Unit_Enter_Interior()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));
            
            InteriorValues vals = new InteriorValues(100, 20, 10, 4);
            Tomb tomb = new Tomb(512, 512, vals);

            unit.EnterInterior(tomb);

            Assert.AreEqual(null, unit.Zone);
            Assert.AreEqual(tomb, unit.Interior);
            Assert.AreEqual(tomb.Entrance.Center, unit.Position);
        }

        [TestMethod]
        public void Unit_Searches_For_Enemies_In_Interior()
        {
            var enemyFaction = GameFrame.Game.Factory.Get<IFaction>();
            var enemyUnit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", enemyFaction));

            InteriorValues vals = new InteriorValues(100, 20, 10, 4);
            Tomb tomb = new Tomb(512, 512, vals);

            _unit.EnterInterior(tomb);
            enemyUnit.EnterInterior(tomb);

            enemyUnit.Position = tomb.InteriorNodes[0].Position;

            var farawayNode =
                tomb.InteriorNodes.First(node => Utility.GetDistance(node.Position, enemyUnit.Position) > 40);
            _unit.Position = farawayNode.Position;

            bool detected = _unit.CombatModule.DetectEnemies();

            Assert.IsFalse(detected);

            farawayNode =
                tomb.InteriorNodes.First(node => Utility.GetDistance(node.Position, enemyUnit.Position) < 15);
            _unit.Position = farawayNode.Position;
            _unit.Position = farawayNode.Position;

            detected = _unit.CombatModule.DetectEnemies();

            Assert.IsTrue(detected);
        }
    }
}
