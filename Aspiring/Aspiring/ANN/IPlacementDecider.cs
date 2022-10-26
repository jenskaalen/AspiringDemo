using System.Collections.Generic;
using AspiringDemo.Factions;
using AspiringDemo.Zones;

namespace AspiringDemo.ANN
{
    public interface IPlacementDecider
    {
        int PreferredCapitalDistance { get; set; }
        int PreferredFactionZoneDistance { get; set; }
        int MaxDistanceFromCapital { get; set; }
        int MinDistanceFromCapital { get; set; }
        int MaxDistanceFromFactionZone { get; set; }
        int MinDistanceFromFactionZone { get; set; }
        IFaction Faction { get; set; }

        IZone GetBestZone(List<IZone> zonesToEvaluate);
    }
}