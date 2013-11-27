using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Gamecore.Helpers;
using AspiringDemo.Units;

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
            var friendlyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(unit.Faction).Relation == RelationType.Friendly);

            var enemyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(unit.Faction).Relation == RelationType.Hostile);

            int friendlyPower = friendlyUnits.Count();
            int enemyPower = enemyUnits.Count();

            if ((friendlyPower + (friendlyPower*ThresholdPercentage/100)) < enemyPower)
                return true;
            else
                return false;
        }

        public static bool WantsToFlee(IFaction faction, INewFight fight)
        {
            // do a simple measurement of faction count
            //fight.
            var friendlyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(faction).Relation == RelationType.Friendly);

            var enemyUnits =
                fight.Units.Where(
                    funit => funit.Faction.Relations.GetRelation(faction).Relation == RelationType.Hostile);

            int friendlyPower = friendlyUnits.Count();
            int enemyPower = enemyUnits.Count();

            if ((friendlyPower + (friendlyPower * ThresholdPercentage / 100)) < enemyPower)
                return true;
            else
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

            var enemySpeed =
                fight.Units.Where(
                    unit => unit.Faction.Relations.GetRelation(faction).Relation == RelationType.Hostile)
                    .Average(unit => unit.Stats.Speed);

            var selfSpeed =
                fight.Units.Where(
                    unit => unit.Faction.Relations.GetRelation(faction).Relation == RelationType.Friendly)
                    .Average(unit => unit.Stats.Speed);

            double speedDifference = selfSpeed - enemySpeed;
            fleeChance += speedDifference;
            fleeChance += (speedDifference * 10);

            return fleeChance;
        }

        public static IZone DetermineRetreatZone(IUnit unit)
        {
            // try to find the area
            if (unit.Faction.Areas.Any())
            {
                IZone retreatZone = Zones.GetClosestZone(unit.Zone.Position,
                    unit.Faction.Areas.Select(area => area.Zone).Where(zone => zone != unit.Zone).ToList());

                return retreatZone;
            }
            else
            {
                //any port in a storm - we pick any neighbor 
                if (unit.Zone.Neighbours.Any())
                    return (IZone) unit.Zone.Neighbours.FirstOrDefault();
                else return null;
            }
        }

        public static void CheckAndPerformFleeing(IFaction faction, INewFight fight)
        {
            if (WantsToFlee(fight.Units.FirstOrDefault(unit => unit.Faction == faction), fight))
            {
                double fleeRoll = 100 - GameFrame.Random.Next(0, 100);
                double chance =  FleeChance(faction, fight);

                if (chance > fleeRoll)
                {
                    // now we flee for real
                    var anyUnit = fight.Units.FirstOrDefault(unit => unit.Faction == faction);
                    var retreatZone = DetermineRetreatZone(anyUnit);

                    if (retreatZone != null)
                        Actions.GiveRetreatOrder(faction, fight, retreatZone);
                }
            }
        }
    }
}
