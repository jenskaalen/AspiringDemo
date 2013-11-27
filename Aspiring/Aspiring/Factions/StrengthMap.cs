using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions
{
    public class StrengthMap
    {
        public List<IFaction>  Factions { get; set; }
        public float NextStrenghtMapping { get; set; }
        public float StrengthMappingInterval { get; set; }

        public StrengthMap()
        {
            StrengthMappingInterval = 5;
            NextStrenghtMapping = 5;
        }

        //TODO: implement a base construcotr call here
        public StrengthMap(float currentTime)
        {
            StrengthMappingInterval = 5;
            NextStrenghtMapping = currentTime + 5;
            Factions = GameFrame.Game.Factions;
            GameFrame.Game.GameTime.TimeTicker += GametickTime;
        }

        public void GametickTime(float time)
        {
            if (time <= NextStrenghtMapping) return;

            MapFactions(Factions);
            NextStrenghtMapping += StrengthMappingInterval;
        }

        public void MapFactions(List<IFaction> factions )
        {
            int highestScore = 0;
            int score = 0;
            int totalscore = 0;

            foreach (var faction in factions)
            {
                score = faction.Army.AliveUnitsCount;
                //TODO: Fix this with regards to units who do not have levels
                //score += faction.Army.Units.Sum(unit => unit.CharacterLevel.Level)/2;

                if (highestScore < score)
                    highestScore = score;

                totalscore += score;
            }

            foreach (var faction in factions)
            {
                score = faction.Army.AliveUnitsCount;
                //TODO: Fix this with regards to units who do not have levels
                //score += faction.Army.Units.Where(unit => unit.State != UnitState.Dead).Sum(unit => unit.CharacterLevel.Level)/2;

                double str = score/( (double) totalscore/factions.Count);
                faction.Strength = GetStrengthMeasurement(str);
            }
        }

        public StrengthMeasurement GetStrengthMeasurement(double strengthRating)
        {
            if (strengthRating > 2)
                return StrengthMeasurement.Hulk;
            else if (strengthRating > 1.4)
                return StrengthMeasurement.Strong;
            else if (strengthRating > 0.8)
                return StrengthMeasurement.Medium;
            else if (strengthRating > 0.4)
                return StrengthMeasurement.Weak;
            else
                return StrengthMeasurement.Abysmal;
        }
    }
}
