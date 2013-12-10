using System;

namespace AspiringDemo.GameObjects.Units
{
    [Serializable]
    public delegate void StateChanged(IUnit unit, UnitState state);
}