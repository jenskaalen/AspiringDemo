using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying
{
    //TODO: Move to a different location?
    public delegate void LevelGain();

    public class CharacterLevel : AspiringDemo.Roleplaying.ICharacterLevel
    {
        public int Level { get; set; }
        public int CurrentXP { get; private set; }
        public int NextLevelXP { get; set; }
        public int StartLevelXP { get; set; }
        public LevelProgressModifier ProgressModifier { get; set; }
        public LevelGain GainLevel { get; set; }

        public int XpWorth
        {
            get
            {
                return Level * _xpBase;
            }
        }

        private int _xpBase = 50;

        public CharacterLevel(LevelProgressModifier progressModifier)
        {
            ProgressModifier = progressModifier;
            GainLevel += LevelUp;
            NextLevelXP = 50;
        }

        public void GainXP(int xp)
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
