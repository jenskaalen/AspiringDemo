using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying.Stats
{
    public interface ICharacterStats
    {
        int CurrentHp { get; set; }
        int MaxHp { get; set; }
        int Speed { get; set; }
        int Strength { get; set; }
        int BaseStrength { get; set; }
        int BaseSpeed { get; set; }
        int BaseHp { get; set; }
        //growth
        int GrowthHp { get; set; }
        int GrowthStrength { get; set; }
        int GrowthSpeed { get; set; }
        int RegenRate { get; set; }
        int RegenHpAmount { get; set; }

        void GainLevel();
        void SetLevel(int level);
        void Regen(long time);
    }
}
