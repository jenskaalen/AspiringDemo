using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat.Attacks
{
    public interface IUnitModifier
    {
        IUnit Source { get; set; }
    }
}