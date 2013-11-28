using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameObjects.Units;

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