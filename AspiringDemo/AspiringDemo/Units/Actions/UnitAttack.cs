using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Planning.Targets;

namespace AspiringDemo.Units.Actions
{
    public class UnitAttack : IUnitAction
    {
        public bool IsFinished { get; set; }

        private IUnit _attacker, _target;

        //no need to work
        public void Work(long time)
        {
            if (_attacker.State == UnitState.Dead || _target.State == UnitState.Dead)
                return;

            int dmg = _attacker.GetDamageOutput();
            _target.Hp -= dmg;

            if (_target.State == UnitState.Dead)
            {
                _attacker.KilledUnit(_target);
            }
        }

        public UnitAttack(IUnit attacker, IUnit target)
        {
            _attacker = attacker;
            _target = target;
        }
    }
}
