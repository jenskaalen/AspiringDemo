using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat.Behaviour
{
    public interface IDetection
    {
        bool DetectEnemy(IUnit searchingUnit, IUnit undetectedUnit);
    }
}