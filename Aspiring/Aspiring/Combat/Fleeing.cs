using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Gamecore.Helpers;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.Combat
{
    public static class Fleeing
    {
        private const double ThresholdPercentage = 70.0;
        private const double InitialFleeChance = 100.0;

        public static bool WantsToFlee(IUnit unit, INewFight fight)
        {
            // do a simple measurement of faction count
            //fight.
            IEnumerable<IUnit> friendlyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(unit.Faction).Relation == RelationType.Friendly);

            IEnumerable<IUnit> enemyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(unit.Faction).Relation == RelationType.Hostile);

            int friendlyPower = friendlyUnits.Count();
            int enemyPower = enemyUnits.Count();

            if ((friendlyPower + (friendlyPower*ThresholdPercentage/100)) < enemyPower)
                return true;
            return false;
        }

        public static bool WantsToFlee(IFaction faction, INewFight fight)
        {
            // do a simple measurement of faction count
            //fight.
            IEnumerable<IUnit> friendlyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(faction).Relation == RelationType.Friendly);

            IEnumerable<IUnit> enemyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(faction).Relation == RelationType.Hostile);

            int friendlyPower = friendlyUnits.Count();
            int enemyPower = enemyUnits.Count();

            if ((friendlyPower + (friendlyPower*ThresholdPercentage/100)) < enemyPower)
                return true;
            return false;
        }

        public static double FleeChance(IFaction faction, INewFight fight)
        {
            if (fight.Units.All(unit => unit.Faction != faction))
                throw new Exception("Faction does not participate in fight");

            // reduced chance by average unit speed compared to enemy
            // reduced chance by large unit counts            
            double fleeChance = InitialFleeChance;
            fleeChance -= fight.Units.Count(unit => unit.Faction == faction);

            double enemySpeed =
                fight.Units.Where(
                    unit => unit.Faction.Relations.GetRelation(faction).Relation == RelationType.Hostile)
                    .Average(unit => unit.Stats.Speed);

            double selfSpeed =
                fight.Units.Where(
                    unit => unit.Faction.Relations.GetRelation(faction).Relation == RelationType.Friendly)
                    .Average(unit => unit.Stats.Speed);

            double speedDifference = selfSpeed - enemySpeed;
            fleeChance += speedDifference;
            fleeChance += (speedDifference*10);

            return fleeChance;
        }

        public static IZone DetermineRetreatZone(IUnit unit)
        {
            // try to find the area
            if (unit.Faction.Areas.Any())
            {
                IZone retreatZone = Zonudes.GetClosestZone(unit.Zone.Position,
                    unit.Faction.Areas.Select(area => area.Zone).Where(zone => zone != unit.Zone).ToList());

                return retreatZone;
            }
            //any port in a storm - we pick any neighbor 
            if (unit.Zone.Neighbours.Any())
                return (IZone) unit.Zone.Neighbours.FirstOrDefault();
            return null;
        }

        public static void CheckAndPerformFleeing(IFaction faction, INewFight fight)
        {
            if (WantsToFlee(fight.Units.FirstOrDefault(unit => unit.Faction == faction), fight))
            {
                double fleeRoll = 100 - GameFrame.Random.Next(0, 100);
                double chance = FleeChance(faction, fight);

                if (chance > fleeRoll)
                {
                    // now we flee for real
                    IUnit anyUnit = fight.Units.FirstOrDefault(unit => unit.Faction == faction);
                    IZone retreatZone = DetermineRetreatZone(anyUnit);

                    if (retreatZone != null)
                        Actions.GiveRetreatOrder(faction, fight, retreatZone);
                }
            }
        }
    }
}