using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Roleplaying
{
    public interface ILeveling
    {
        ICharacterLevel CharacterLevel { get; set; }
        void Loot(IUnit unit);
    }
}