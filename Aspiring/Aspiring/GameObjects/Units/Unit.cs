using System;
using AspiringDemo.Factions;
using AspiringDemo.Roleplaying;

namespace AspiringDemo.GameObjects.Units
{
    //TODO: Extract interface
    [Serializable]
    public sealed class Unit : BaseUnit, IUnitLeveling
    {
        public Unit(IFaction faction) : base(faction)
        {
            Stats.Speed = 20;
            Hp = 25;
            XPWorth = 50;
            CharacterLevel = new CharacterLevel(new LevelProgressModifier());
            CharacterLevel.GainLevel += Stats.GainLevel;
            Name = "Soldier";
        }

        public ICharacterLevel CharacterLevel { get; set; }

        public override void KilledUnit(IUnit unit)
        {
            base.KilledUnit(unit);
            Loot(unit);
            CharacterLevel.GainXP(unit.XPWorth);
        }
    }
}