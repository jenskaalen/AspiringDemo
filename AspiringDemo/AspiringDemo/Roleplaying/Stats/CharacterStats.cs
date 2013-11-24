using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying.Stats
{
    public class CharacterStats : ICharacterStats
    {
        public int CurrentHp
        {
            get { return _currentHp; }
            set
            {
                if (value > _maxHp)
                    _currentHp = _maxHp;
                else
                    _currentHp = value;
                //_currentHp = _currentHp + value > _maxHp ? _maxHp : value;
                //_currentHp = value;
            }
        }

        public int MaxHp
        {
            get { return _maxHp; }
            set { _maxHp = value; }
        }

        public int Speed { get; set; }

        public int Strength { get; set; }

        public int BaseStrength { get; set; }

        public int BaseSpeed { get; set; }
        public int BaseHp
        {
            get { return _baseHp; }
            set { _baseHp = value; }
        }

        public int GrowthHp
        {
            get { return _growthHp; }
            set { _growthHp = value; }
        }

        public int GrowthStrength { get; set; }

        public int GrowthSpeed { get; set; }

        public int RegenRate { get; set; }

        public int RegenHpAmount { get; set; }

        private int _currentLevel = 0;
        private int _growthHp = 5;
        private int _baseHp = 25;
        private int _maxHp = 25;
        private int _currentHp;
        private int _nextRegen;

        public CharacterStats()
        {
            BaseSpeed = 0;
            BaseStrength = 0;
            Strength = 0;
            Speed = 0;
            GrowthSpeed = 1;
            GrowthStrength = 1;
            RegenRate = 10;
            RegenHpAmount = 5;
            _nextRegen = RegenRate;
        }

        public void GainLevel()
        {
            //TODO: Is this the correct way to increase levels?
            _currentLevel++;
            _maxHp = _baseHp + (_growthHp * _currentLevel);
            Strength += GrowthStrength;
            Speed += GrowthSpeed;

            //TODO: Fix this, but let's say for now that a current gets healed by half his hp when he gains a level
            CurrentHp += (MaxHp/2);
        }

        public void SetLevel(int level)
        {
            _currentHp = level;
            _maxHp = _baseHp + (_growthHp * _currentLevel);
            Strength = BaseStrength + (GrowthStrength*_currentLevel);
            Speed = BaseSpeed + (GrowthSpeed * _currentLevel);

            //TODO: Fix this, but let's say for now that a current gets healed by half his hp when he gains a level
            CurrentHp += (MaxHp / 2);
        }

        public void Regen(long time)
        {
            if (time >= _nextRegen)
            {
                CurrentHp += RegenHpAmount;
                _nextRegen += RegenRate;
            }
        }
    }
}
