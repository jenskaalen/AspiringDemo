using System;
namespace AspiringDemo.Roleplaying
{
    public interface ICharacterLevel
    {
        int CurrentXP { get; }
        void GainXP(int xp);
        int Level { get; set; }
        int NextLevelXP { get; set; }
        LevelProgressModifier ProgressModifier { get; set; }
        int StartLevelXP { get; set; }
        LevelGain GainLevel { get; set; }
    }
}
