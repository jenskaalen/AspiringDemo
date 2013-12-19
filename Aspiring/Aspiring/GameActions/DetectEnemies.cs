using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameActions.Combat;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions
{
    /// <summary>
    /// Continiously search for enemies. If enemy is found then the enemy will be attacked.
    /// </summary>
    public class DetectEnemies : GameAction
    {
        private IUnit _unit;

        public DetectEnemies(IUnit unit)
        {
            _unit = unit;
        }

        public override void Update(float elapsed)
        {
            if (_unit.State == UnitState.Fighting)
                return;

            if (_unit.Zone.Type != ZoneType.Interior)
            {
                Finished = true;
                return;
            }

            bool detected = _unit.CombatModule.DetectEnemies();

            // find all enemies in range
            //NOTE: this might possibly target allies?
            var enemies = _unit.Zone.Units.Where(unit => _unit.Faction != unit.Faction && _unit.CombatModule.DetectEnemy(unit));

            if (enemies.Any())
            {
                // attack any enemy
                var attack = new UnitAttack(_unit, enemies.First());
                _unit.Actions.Add(attack);
            }
        }
    }
}
