using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Units;

namespace AspiringDemo.Gamecore.Helpers
{
    public static class Relations
    {
        public static bool IsEnemy(this IUnit thisUnit, IUnit unit)
        {
            return thisUnit.Faction.Relations.GetRelation(unit.Faction).Relation == RelationType.Hostile;
        }
    }
}
