using System.Collections.Generic;
using System.Xml.Linq;
using AspiringDemo;
using AspiringDemo.Factions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace AspiringDemoTest.Creator
{
    [TestClass]
    public class Creation
    {
        [TestMethod]
        public void CreateUnit()
        {
            var creator = new AspiringDemo.Gamecore.XmlCreator();
            XDocument xdoc = XDocument.Load("Creator\\units.xml");

            var xunit = new XElement("Unit");
            xunit.Add(new XElement("Faction", "testFaction"));
            xdoc.Root.Add(xunit);
            xdoc.Save("Creator\\units.xml");
            
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            GameFrame.Game.Factions = new List<IFaction>();
            GameFrame.Game.Factions.Add(faction);

            faction.Name = "testFaction";

            creator.ReadXml("Creator\\units.xml");

            Assert.AreEqual(1, faction.Army.AliveUnits.Count);
        }
    }
}
