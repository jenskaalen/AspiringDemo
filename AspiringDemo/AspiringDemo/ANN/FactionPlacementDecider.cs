using AspiringDemo.Factions;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN
{
    public class FactionPlacementDecider : IPlacementDecider
    {
        public int PreferredCapitalDistance { get; set; }
        public int PreferredFactionZoneDistance { get; set; }
        public int MaxDistanceFromCapital { get; set; }
        public int MinDistanceFromCapital { get; set; }
        public int MaxDistanceFromFactionZone { get; set; }
        public int MinDistanceFromFactionZone { get; set; }
        // custom
        public IFaction Faction { get; set; }
        private List<IZone> _zones;
        private List<IZone> _factionZones;
        private List<IZone> _openZones;

        public IZone GetBestZone(List<IZone> zonesToEvaluate)
        {
            IZone bestZone = null;
            int lowestScore = int.MaxValue;
            int lastScore = 0;

            _zones = zonesToEvaluate;
            _factionZones = zonesToEvaluate.Where(x => x.PopulatedAreas.Any()).ToList();
            _openZones = zonesToEvaluate.Where(x => !x.PopulatedAreas.Any()).ToList();

            foreach (var zone in _openZones)
            {
                lastScore = ZoneScore(zone);

                if (lastScore < lowestScore)
                {
                    lowestScore = lastScore;
                    bestZone = zone;
                }
            }

            return bestZone;
        }

        private int ZoneScore(IZone zone)
        {
            IZone capitalZone = Faction.CapitalZone;
            int score = 0;

            int capitalDist = (int) zone.DistanceToNode(capitalZone);

            if (capitalDist > MaxDistanceFromCapital || capitalDist < MinDistanceFromCapital)
            {
                return int.MaxValue;
            }

            score = capitalDist - PreferredCapitalDistance;
            int lowestZoneValue = int.MaxValue;
            int calcValue = 0;

            // checking closeness to any faction nodes
            //TODO: Check if this can be done in a more efficient way
            foreach (Zone factionZone in _factionZones)
            {
                int distanceToZone = (int)(zone.DistanceToNode(factionZone));

                if (distanceToZone > MaxDistanceFromFactionZone || distanceToZone < MinDistanceFromFactionZone)
                    calcValue = int.MaxValue;
                else 
                    calcValue = distanceToZone - PreferredFactionZoneDistance;

                if (calcValue < lowestZoneValue)
                    lowestZoneValue = calcValue;
            }

            score += lowestZoneValue;

            return Math.Abs(score);
        }
    }
}
