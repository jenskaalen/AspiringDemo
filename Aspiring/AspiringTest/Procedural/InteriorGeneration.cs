using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Procedural;
using AspiringDemo.Zones.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace AspiringDemoTest.Procedural
{
    [TestClass]
    public class InteriorGeneration
    {
        //[TestMethod]
        public void Generate_Tomb()
        {
            var vals = new InteriorValues(10, 20, 10, 4);
            var tomb = new Tomb(512, 512, vals);

            Assert.AreEqual(10, tomb.Rooms.Count);
            Assert.IsTrue(tomb.Paths.Any());
        }

        //[TestMethod]
        public void Populate_Tomb()
        {
            var vals = new InteriorValues(10, 20, 10, 4);
            var tomb = new Tomb(512, 512, vals);
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var generator = new TombCreatureGenerator(20, faction);

            generator.Populate(tomb);

            Assert.IsTrue(generator.Creatures.All(unit => unit.Zone == tomb));
            Assert.AreEqual(20, tomb.Units.Count);
        }

        //[TestMethod]
        public void Interior_Debug_Image()
        {
            var vals = new InteriorValues(60, 60, 30, 7);
            var tomb = new Tomb(1024, 1024, vals);
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var generator = new TombCreatureGenerator(30, faction);
            generator.Populate(tomb);

            tomb.CreateDebugImage();
        }
    }
}
