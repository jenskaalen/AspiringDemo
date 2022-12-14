using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat
{
    public interface ICombatModule
    {
        /// <summary>
        ///     If set to false - the unit will never, ever flee
        /// </summary>
        bool CanFlee { get; set; }

        /// <summary>
        ///     The higher the value, higher the chance of the unit not entering the fight.
        ///     Zero (0) value means the unit always will enter to fight
        /// </summary>
        int CombatReluctance { get; set; }

        int Kills { get; set; }
        INewFight CurrentFight { get; set; }
        IUnit CurrentTarget { get; set; }
        double DetectionDistance { get; set; }
        void AttackTarget(IUnit target, float time);
        IUnit GetTarget(List<IUnit> potentialTargets);
        List<IUnit> GetPotentialTargets(List<IUnit> units);

        /// <summary>
        ///     Alerts units in zone
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="fight"></param>
        void ShoutForHelp();
        bool DetectEnemies();
        /// <summary>
        /// Checks if the enemy is within detection range
        /// </summary>
        /// <param name="targetUnit"></param>
        /// <returns></returns>
        bool DetectEnemy(IUnit targetUnit);
    }
}