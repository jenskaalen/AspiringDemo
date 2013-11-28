using System.Collections.Generic;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Factions
{
    public interface IArmy
    {
        //TODO: Do something about lists being open to externally adding/removing
        List<ISquad> Squads { get; }
        List<IUnit> Units { get; }
        //TODO: Make this one obsolete and remove
        int AliveUnitsCount { get; }
        List<IUnit> AliveUnits { get; }
        List<IUnit> GetIdleUnits();
        ISquad GetUnitSquad(IUnit unit);
        void AddSquad(ISquad squad);
        void RemoveSquad(ISquad squad);
        void AddUnit(IUnit unit);
        void RemoveUnit(IUnit unit);
    }
}