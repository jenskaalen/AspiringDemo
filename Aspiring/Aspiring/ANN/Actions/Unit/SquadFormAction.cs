using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.ANN.Actions.Unit
{
    [Serializable]
    internal class SquadFormAction
    {
        private const int PreferredSquadSize = 3;

        /// <summary>
        ///     Forms a squad if there are available units
        /// </summary>
        /// <param name="faction"></param>
        /// <returns>Return the formed squad, or null if no squad is found</returns>
        public static ISquad FormSquad(IFaction faction)
        {
            //TODO: oh god, optimize this
            List<IUnit> possibleSquadUnits =
                faction.Army.AliveUnits.Where(unit => unit.Squad == null && unit.State == UnitState.Idle).ToList();

            if (possibleSquadUnits.Count >= PreferredSquadSize)
            {
                ISquad squad = faction.CreateSquad();

                foreach (IUnit unit in possibleSquadUnits.Take(PreferredSquadSize))
                {
                    squad.AddMember(unit);
                }

                return squad;
            }
            return null;
        }

        public static ISquad FormSquad(IFaction faction, int squadSize)
        {
            //TODO: oh god, optimize this
            List<IUnit> possibleSquadUnits =
                faction.Army.AliveUnits.Where(unit => unit.Squad == null && unit.State == UnitState.Idle).ToList();

            if (possibleSquadUnits.Count >= squadSize)
            {
                ISquad squad = faction.CreateSquad();

                foreach (IUnit unit in possibleSquadUnits.Take(PreferredSquadSize))
                {
                    squad.AddMember(unit);
                }

                return squad;
            }
            return null;
        }
    }
}