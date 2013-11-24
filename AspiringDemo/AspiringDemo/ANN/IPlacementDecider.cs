using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //TODO: Make zone fully izone compliant
        // and replace concretions of izone
        IZone GetBestZone(List<IZone> zonesToEvaluate);
    }
}
