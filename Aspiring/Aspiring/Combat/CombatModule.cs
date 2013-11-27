using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore;
using AspiringDemo.Units;

namespace AspiringDemo.Combat
{
    public class CombatModule : ICombatModule
    {
        public bool CanFlee { get; set; }
        public int CombatReluctance { get; set; }
        public int Kills { get; set; }

        private float _nextAttack;
        private const float SpeedModifier = 0.1f;

        private IUnit _unit;

        public void AttackTarget(IUnit target)
        {
            if (target.State == UnitState.Dead)
                return;

            //TODO: refactor this into some kind of attack
            var bestWeapon = _unit.Items.GetBestWeapon();

            if (_unit.Items.CurrentWeapon != bestWeapon)
                ChangeWeapon(_unit.Items.GetBestWeapon());

            if (_nextAttack < GameFrame.Game.GameTime.Time)
            {
                SetNextAttack();
                // do attack...
                target.Hp -= _unit.Items.CurrentWeapon.BaseDamage + _unit.Stats.Strength;
            }
        }

        public void EnterFight(NewFight fight)
        {

        }

        public void ChangeWeapon(IWeapon weapon)
        {
            _unit.Items.EquipWeapon(weapon);
        }

        private void SetNextAttack()
        {
            _nextAttack = (_unit.Items.CurrentWeapon.WeaponSpeed / _unit.Stats.Speed) * SpeedModifier + GameFrame.Game.GameTime.Time;
        }

        public CombatModule(IUnit unit)
        {
            _unit = unit;
        }
    }
}
