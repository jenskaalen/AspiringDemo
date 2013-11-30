using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.Procedural;
using AspiringDemo.Procedural.Interiors;
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

            var tomb = new Tomb(10, 512, 512);
            tomb.Populate(generator);

            foreach (var room in tomb.Rooms)
            {
                
            }
        }
    }
}
