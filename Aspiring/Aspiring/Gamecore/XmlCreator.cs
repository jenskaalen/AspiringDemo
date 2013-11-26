using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspiringDemo.Factions;
using AspiringDemo.Units;

namespace AspiringDemo.Gamecore
{
    public class XmlCreator
    {
        public void ReadXml(string xmlFile)
        {
            XDocument xml = null;

            try
            {
                xml = XDocument.Load(xmlFile);
            }
            catch
            {}
                
            if (xml != null)
                ReadXml(xml, xmlFile);
        }

        public void ReadXml(XDocument xmlFile, string filename)
        {
            if (!xmlFile.Descendants("Unit").Any())
                return;

            foreach (var unitele in xmlFile.Descendants("Unit"))
            {
                CreateUnit(unitele);
            }

            xmlFile.Descendants("Unit").Remove();
            xmlFile.Save(filename);
        }

        private void CreateUnit(XElement unitElement)
        {
            var xElement = unitElement.Element("Faction");
            if (xElement != null)
            {
                var factionName = xElement.Value;
                IFaction faction = GameFrame.Game.Factions.FirstOrDefault(x => x.Name == factionName);

                if (faction != null)
                {
                    var unit = (Unit) faction.CreateUnit();
                    //unit.CharacterLevel.GainXP();
                    unit.Name = unitElement.Element("Name") != null ? unitElement.Element("Name").Value : unit.Name;
                    unit.Hp = unitElement.Element("Hp") != null ? int.Parse(unitElement.Element("Hp").Value) : unit.Hp;

                    if (unitElement.Element("Level") != null)
                    {
                        int count = int.Parse(unitElement.Element("Level").Value);
                        for (int i=0; i < count; i++)
                            unit.CharacterLevel.GainLevel();

                    }
                }
            }
        }
    }
}
