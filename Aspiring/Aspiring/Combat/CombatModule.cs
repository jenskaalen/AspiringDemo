using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Gamecore.Helpers;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat
{
    public class CombatModule : ICombatModule
    {
        private const float SpeedModifier = 0.1f;
        private readonly IUnit _unit;
        private float _nextAttack;

        public CombatModule(IUnit unit)
        {
            _unit = unit;
        }

        public bool CanFlee { get; set; }
        public int CombatReluctance { get; set; }
        public int Kills { get; set; }
        public INewFight CurrentFight { get; set; }
        public IUnit CurrentTarget { get; set; }

        public void AttackTarget(IUnit target, float time)
        {
            if (target.State == UnitState.Dead)
                return;

            //TODO: refactor this into some kind of attack
            IWeapon bestWeapon = _unit.Items.GetBestWeapon();

            if (_unit.Items.CurrentWeapon != bestWeapon)
                ChangeWeapon(_unit.Items.GetBestWeapon());

            if (_nextAttack < time)
            {
                SetNextAttack(time);
                // do attack...
                target.Hp -= _unit.Items.CurrentWeapon.BaseDamage + _unit.Stats.Strength;
                //IF WE KILLED 'IM
                if (target.State == UnitState.Dead)
                {
                    _unit.KilledUnit(target);
                }
            }
        }

        public IUnit GetTarget(List<IUnit> potentialTargets)
        {
            // lol, we pick random (...)
            IUnit unit = potentialTargets[GameFrame.Random.Next(0, potentialTargets.Count)];
            return unit;
        }

        public List<IUnit> GetPotentialTargets(List<IUnit> units)
        {
            return units.Where(unit => _unit.IsEnemy(unit)).ToList();
        }

        public void ShoutForHelp()
        {
            if (_unit.CombatModule.CurrentFight == null)
                throw new Exception("Cant shout from help because unit is not in a fight.");

            if (_unit.Zone == null) return;

            foreach (IUnit ally in _unit.Zone.Units.Where(fightUnit =>
                fightUnit.CombatModule.CurrentFight == null
                && _unit.Faction.Relations.Allies.Contains(fightUnit.Faction)))
            {
                CurrentFight.Enter(ally);
            }
        }

        public void ChangeWeapon(IWeapon weapon)
        {
            _unit.Items.EquipWeapon(weapon);
        }

        private void SetNextAttack(float time)
        {
            _nextAttack = (_unit.Items.CurrentWeapon.WeaponSpeed/_unit.Stats.Speed)*SpeedModifier + time;
        }
    }
}