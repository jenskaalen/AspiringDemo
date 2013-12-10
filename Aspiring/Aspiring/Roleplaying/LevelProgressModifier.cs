using System;

namespace AspiringDemo.Roleplaying
{
    [Serializable]
    public class LevelProgressModifier
    {
        public LevelProgressModifier()
        {
            XpMultiplier = 1.5;
        }

        public double XpMultiplier { get; set; }

        public int GetRequiredXpNextLevel(long currentLevelXp)
        {
            if (currentLevelXp == 0)
                throw new Exception("currentLevelXp cant be 0!");

            return (int) (currentLevelXp*XpMultiplier);
        }
    }
}