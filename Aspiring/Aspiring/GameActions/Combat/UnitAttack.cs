using AspiringDemo.Units;

namespace AspiringDemo.GameActions.Combat
{
    public class UnitAttack : GameAction
    {
        private IUnit _attacker, _target;

        public UnitAttack(IUnit attacker, IUnit target)
        {
            _attacker = attacker;
            _target = target;

            _attacker.CombatModule.CurrentTarget = target;
        }

        public override void Update(float elapsed)
        {
            if (_attacker.State == UnitState.Dead || _target.State == UnitState.Dead)
            {
                Finished = true;
                _attacker.CombatModule.CurrentTarget = null;
                return;
            }

            _attacker.CombatModule.AttackTarget(_target, elapsed);
        }
    }
}
