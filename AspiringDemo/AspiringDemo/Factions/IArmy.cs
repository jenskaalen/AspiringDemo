using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Factions
{
    public interface IArmy
    {
        //TODO: Do something about lists being open to externally adding/removing
        List<ISquad> Squads { get; }
        List<IUnit>  Units{ get; }
        List<IUnit> GetIdleUnits();
        ISquad GetUnitSquad(IUnit unit);
        //TODO: Make this one obsolete and remove
        int AliveUnitsCount { get; }
        List<IUnit> AliveUnits { get;  } 
        void AddSquad(ISquad squad);
        void RemoveSquad(ISquad squad);
        void AddUnit(IUnit unit);
        void RemoveUnit(IUnit unit);
    }
}
