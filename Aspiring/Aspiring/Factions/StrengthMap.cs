using System;
using System.Collections.Generic;

namespace AspiringDemo.Factions
{
    [Serializable]
    public class StrengthMap
    {
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

        public List<IFaction> Factions { get; set; }
        public float NextStrenghtMapping { get; set; }
        public float StrengthMappingInterval { get; set; }

        public void GametickTime(float time)
        {
            if (time <= NextStrenghtMapping) return;

            MapFactions(Factions);
            NextStrenghtMapping += StrengthMappingInterval;
        }

        public void MapFactions(List<IFaction> factions)
        {
            int highestScore = 0;
            int score = 0;
            int totalscore = 0;

            foreach (IFaction faction in factions)
            {
                score = faction.Army.AliveUnitsCount;
                //TODO: Fix this with regards to units who do not have levels
                //score += faction.Army.Units.Sum(unit => unit.CharacterLevel.Level)/2;

                if (highestScore < score)
                    highestScore = score;

                totalscore += score;
            }

            foreach (IFaction faction in factions)
            {
                score = faction.Army.AliveUnitsCount;
                //TODO: Fix this with regards to units who do not have levels
                //score += faction.Army.Units.Where(unit => unit.State != UnitState.Dead).Sum(unit => unit.CharacterLevel.Level)/2;

                double str = score/((double) totalscore/factions.Count);
                faction.Strength = GetStrengthMeasurement(str);
            }
        }

        public StrengthMeasurement GetStrengthMeasurement(double strengthRating)
        {
            if (strengthRating > 2)
                return StrengthMeasurement.Hulk;
            if (strengthRating > 1.4)
                return StrengthMeasurement.Strong;
            if (strengthRating > 0.8)
                return StrengthMeasurement.Medium;
            if (strengthRating > 0.4)
                return StrengthMeasurement.Weak;
            return StrengthMeasurement.Abysmal;
        }
    }
}