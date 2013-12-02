using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Procedural;
using AspiringDemo.Zones.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace AspiringDemoTest.Procedural
{
    [TestClass]
    public class CreatureGeneration
    {
        [TestMethod]
        public void Tomb_Generation_Units()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var generator = new TombCreatureGenerator(10, faction);
            Assert.AreEqual(10, generator.Creatures.Count);
        }

        [TestMethod]
        public void Creature_Placement_In_Interior()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var generator = new TombCreatureGenerator(10, faction);

            InteriorValues vals = new InteriorValues(100, 20, 10, 4);
            var tomb = new Tomb(0, 0,512, 512, vals);
            tomb.Populate(generator);

            foreach (var room in tomb.Rooms)
            {
                
            }
        }
    }
}
