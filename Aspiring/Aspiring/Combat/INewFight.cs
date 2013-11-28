using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat
{
    public interface INewFight
    {
        List<IUnit> Units { get; set; }
        bool Finished { get; set; }
        void Leave(IUnit unit);
        void Enter(IUnit unit);
    }
}