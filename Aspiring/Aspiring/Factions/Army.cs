using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Factions
{
    public class Army : IArmy
    {
        public List<ISquad> Squads
        {
            get;
            set;
        }

        public List<IUnit> Units
        {
            get;
            set;
        }

        public Army()
        {
            Squads = new List<ISquad>();
            Units = new List<IUnit>();
        }

        public List<IUnit> AliveUnits
        {
            get { return Units.Where(unit => unit.State != UnitState.Dead).ToList(); }
        } 


        public int AliveUnitsCount
        {
            get { return Units.Count(unit => unit.State != UnitState.Dead); }
        }

        /// <summary>
        /// Returns all units considered idle
        /// </summary>
        /// <returns></returns>
        public List<IUnit> GetIdleUnits()
        {
            //TOOD: Optimalize this
            var units = new List<IUnit>();

            foreach (var unit in Units.Where(x => x.State == UnitState.Idle && x.Order == null))
            {
                if (Squads.Count == 0 || Squads.Any(x => !x.Members.Contains(unit)))
                    units.Add(unit);
            }

            return units;
        }

        public ISquad GetUnitSquad(IUnit unit)
        {
            var squad = Squads.FirstOrDefault(x => x.Members.Contains(unit));
            return squad;
        }


        public void AddSquad(ISquad squad)
        {
            Squads.Add(squad);
        }

        public void RemoveSquad(ISquad squad)
        {
            Squads.Remove(squad);
        }

        public void AddUnit(IUnit unit)
        {
            Units.Add(unit);
        }

        public void RemoveUnit(IUnit unit)
        {
            Units.Remove(unit);
        }
    }
}
