using System.Collections.Generic;
using AspiringDemo.Units;

namespace AspiringDemo.Combat
{
    public interface IFight
    {
        List<IUnit> FightingUnits { get; }
        bool FightActive { get; set; }
        int RoundsOfFighting { get; set; }
        int FightersCount { get; }
        void PerformFightRound();
        List<IUnit> GetViableTargets(IUnit unit);
        void AddUnit(IUnit unit);
        void LeaveFight(IUnit unit);
    }
}