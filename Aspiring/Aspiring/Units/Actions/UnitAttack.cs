using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameActions;
using Ninject.Planning.Targets;

namespace AspiringDemo.Units.Actions
{
    public class UnitAttack : GameAction
    {
        private IUnit _attacker, _target;

        public UnitAttack(IUnit attacker, IUnit target)
        {
            _attacker = attacker;
            _target = target;
        }

        public override void Update(float elapsed)
        {
            if (_attacker.State == UnitState.Dead || _target.State == UnitState.Dead)
            {
                Finished = true;
                return;
            }

            _attacker.CombatModule.AttackTarget(_target);
        }
    }
}
