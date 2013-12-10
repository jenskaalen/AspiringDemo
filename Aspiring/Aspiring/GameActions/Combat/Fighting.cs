using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Combat;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.GameActions.Combat
{
    /// <summary>
    ///     A unit will have this action as long as it is fighting with anyone
    /// </summary>
    [Serializable]
    public class Fighting : CompositeAction
    {
        private readonly INewFight _fight;
        private readonly IUnit _unit;

        public Fighting(INewFight fight, IUnit unit)
        {
            Actions = new List<GameAction>();
            _fight = fight;
            _unit = unit;
            _unit.CombatModule.CurrentFight = fight;
            _unit.State = UnitState.Fighting;
            _unit.CombatModule.ShoutForHelp();
        }

        public override void Update(float elapsed)
        {
            Actions.ForEach(a => a.Update(elapsed));
            Actions.RemoveAll(a => a.Finished);
            if (_unit.CombatModule.CurrentTarget == null ||
                _unit.CombatModule.CurrentTarget.CombatModule.CurrentFight == _fight)
            {
                List<IUnit> targets = _unit.CombatModule.GetPotentialTargets(_fight.Units);

                if (targets.Any())
                {
                    var attack = new UnitAttack(_unit, _unit.CombatModule.GetTarget(targets));
                    Actions.Add(attack);
                }
            }

            Finished = Actions.Count == 0;

            if (Finished)
            {
                _fight.Leave(_unit);
            }
        }
    }
}