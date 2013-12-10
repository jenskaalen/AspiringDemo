using System;

namespace AspiringDemo.Roleplaying
{
    //TODO: Move to a different location?
    [Serializable]
    public delegate void LevelGain();

    [Serializable]
    public class CharacterLevel : ICharacterLevel
    {
        private int _xpBase = 50;

        public CharacterLevel(LevelProgressModifier progressModifier)
        {
            ProgressModifier = progressModifier;
            GainLevel += LevelUp;
            NextLevelXP = 50;
        }

        public long XpWorth
        {
            get { return Level*_xpBase; }
        }

        public long Level { get; set; }
        public long CurrentXP { get; private set; }
        public long NextLevelXP { get; set; }
        public long StartLevelXP { get; set; }
        public LevelProgressModifier ProgressModifier { get; set; }
        public LevelGain GainLevel { get; set; }

        public void GainXP(long xp)
        {
            CurrentXP += xp;

            if (CurrentXP >= NextLevelXP)
            {
                GainLevel();
            }
        }

        private void LevelUp()
        {
            NextLevelXP = ProgressModifier.GetRequiredXpNextLevel(NextLevelXP);

            if (CurrentXP > NextLevelXP)
                GainLevel();

            Level++;
        }
    }
}