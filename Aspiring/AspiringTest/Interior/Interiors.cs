using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Procedural;
using AspiringDemo.Procedural.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest.Interior
{
    [TestClass]
    public class Interiors
    {
        [TestMethod]
        public void Unit_Enter_Interior()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));

            Tomb tomb = new Tomb(10);

            unit.EnterInterior(tomb);

            Assert.AreEqual(null, unit.Zone);
            Assert.AreEqual(tomb, unit.Interior);
        }
    }
}
