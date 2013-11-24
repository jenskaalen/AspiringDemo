using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Combat
{
    public interface ICombatBehaviour
    {
        /// <summary>
        /// If set to false - the unit will never, ever flee
        /// </summary>
        bool CanFlee { get; set; }
        /// <summary>
        /// The higher the value, higher the chance of the unit not entering the fight.
        /// Zero (0) value means the unit always will enter to fight
        /// </summary>
        int CombatReluctance { get; set; }
    }
}
