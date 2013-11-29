using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Factions
{
    public class FactionUnits : IFactionUnits
    {
        public List<IUnit> Units { get; set; }
    }
}
