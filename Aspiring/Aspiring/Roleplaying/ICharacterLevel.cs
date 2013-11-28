namespace AspiringDemo.Roleplaying
{
    public interface ICharacterLevel
    {
        long CurrentXP { get; }
        long Level { get; set; }
        long NextLevelXP { get; set; }
        long StartLevelXP { get; set; }
        LevelProgressModifier ProgressModifier { get; set; }
        LevelGain GainLevel { get; set; }
        void GainXP(long xp);
    }
}