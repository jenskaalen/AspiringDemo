using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Factions
{
    interface IArmyUnit
    {
        IUnit Unit { get; set; }
        ISquad Squad { get; set; }
    }
}
