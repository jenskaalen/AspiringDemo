using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.Units;

namespace AspiringDemo.Combat
{
    public class NewFight : INewFight
    {
        public List<IUnit> Units { get; set; }
        public bool Finished { get; set; }

        public NewFight()
        {
            Units = new List<IUnit>();
        }

        public void Leave(IUnit unit)
        {
            Units.Remove(unit);
            unit.CombatModule.CurrentFight = null;
            unit.State = UnitState.Idle;

            if (IsFightFinished())
                Finished = true;
        }

        public void Enter(IUnit unit)
        {
            Units.Add(unit);

            //creating action and adding it to... ?
            GameFrame.Game.ActionProcesser.Actions.Add
                (
                    new Fighting(this, unit)
                );
        }

        private bool IsFightFinished()
        {
            return FactionRelations.ContainsHostileFactions(Units.Select(unit => unit.Faction).Distinct());
        }
    }
}
