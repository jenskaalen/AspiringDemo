using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat
{
    public class NewFight : INewFight
    {
        public NewFight()
        {
            Units = new List<IUnit>();
        }

        public List<IUnit> Units { get; set; }
        public bool Finished { get; set; }

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