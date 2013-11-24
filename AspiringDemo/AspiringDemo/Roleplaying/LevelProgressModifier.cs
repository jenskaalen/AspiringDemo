using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo.Roleplaying
{
    public class LevelProgressModifier
    {
        public LevelProgressModifier()
        {
            XpMultiplier = 1.5;
        }

        public double XpMultiplier { get; set; }

        public int GetRequiredXpNextLevel(int currentLevelXp)
        {
            if (currentLevelXp == 0)
                throw new Exception("currentLevelXp cant be 0!");

            return (int)(currentLevelXp * XpMultiplier);
        }
    }
}
