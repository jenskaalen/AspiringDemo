using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Factions
{
    /// <summary>
    /// For the moment just used for keeping track of units that are not in the army
    /// </summary>
    public interface IFactionUnits
    {
        List<IUnit> Units { get; set; }
    }
}
