using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Orders;
using AspiringDemo.Units;

namespace AspiringDemo.ANN.Actions.Unit
{
    class SquadFormAction
    {
        private const int PreferredSquadSize = 3;

        /// <summary>
        /// Forms a squad if there are available units
        /// </summary>
        /// <param name="faction"></param>
        /// <returns>Return the formed squad, or null if no squad is found</returns>
        public static ISquad FormSquad(IFaction faction)
        {
            //TODO: oh god, optimize this
            var possibleSquadUnits = faction.Army.AliveUnits.Where(unit => unit.Squad == null && unit.State == UnitState.Idle).ToList();

            if (possibleSquadUnits.Count >= PreferredSquadSize)
            {
                ISquad squad = faction.CreateSquad();

                foreach (var unit in possibleSquadUnits.Take(PreferredSquadSize))
                {
                    squad.AddMember(unit);
                }

                return squad;
            }
            else
            {
                return null;
            }
        }

        public static ISquad FormSquad(IFaction faction, int squadSize)
        {
            //TODO: oh god, optimize this
            var possibleSquadUnits = faction.Army.AliveUnits.Where(unit => unit.Squad == null && unit.State == UnitState.Idle).ToList();

            if (possibleSquadUnits.Count >= squadSize)
            {
                ISquad squad = faction.CreateSquad();

                foreach (var unit in possibleSquadUnits.Take(PreferredSquadSize))
                {
                    squad.AddMember(unit);
                }

                return squad;
            }
            else
            {
                return null;
            }
        }
    }
}
