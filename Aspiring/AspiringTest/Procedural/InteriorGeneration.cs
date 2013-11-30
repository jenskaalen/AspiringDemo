using AspiringDemo.Procedural.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemoTest.Procedural
{
    [TestClass]
    public class InteriorGeneration
    {
        [TestMethod]
        public void Generate_Tomb()
        {
            var tomb = new Tomb(10, 512, 512);

            Assert.AreEqual(10, tomb.Rooms.Count);
            Assert.IsTrue(tomb.Paths.Any());
        }
    }
}
