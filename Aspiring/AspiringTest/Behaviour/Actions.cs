using System.Collections.Generic;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameActions;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest.Behaviour
{
    [TestClass]
    public class Actions
    {
        private IUnit _unit1, _unit2;
        private IFaction _faction1, _faction2;
        private List<IZone> _zones;

        [TestInitialize]
        public void Initialize()
        {
            _faction1 = GameFrame.Game.Factory.Get<IFaction>();
            _faction2 = GameFrame.Game.Factory.Get<IFaction>();
            _unit1 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", _faction1));
            _unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", _faction2));

            GameFrame.Game.Initialize();
            GameFrame.Game.ZonePathfinder.Nodes = Factories.GetZones(9, 9);
            _zones = GameFrame.Game.ZonePathfinder.Nodes;
        }

        [TestMethod]
        public void Zone_Move_Action()
        {
            var from = _zones[0];
            _unit1.EnterZone(from);

            var to = _zones[5];

            var moveAction = new ZoneMove(_unit1, from, to);

            for (int i = 0; i < 7; i++)
            {
                moveAction.Update(i);
            }

            Assert.AreEqual(true, moveAction.Finished);
            Assert.AreEqual(to, _unit1.Zone);
        }
    }
}
