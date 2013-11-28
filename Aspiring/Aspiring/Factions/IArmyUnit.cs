using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Factions
{
    internal interface IArmyUnit
    {
        IUnit Unit { get; set; }
        ISquad Squad { get; set; }
    }
}