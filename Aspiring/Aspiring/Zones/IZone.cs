using System;
using System.Collections.Generic;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Sites;

namespace AspiringDemo.Zones
{
    public interface IZone : IPathfindingNode
    {
        int ID { get; set; }
        bool IsPlayerNearby { get; set; }
        List<IPopulatedArea> PopulatedAreas { get; set; }
        Rect Area { get; set; }
        ZoneType Type { get; set; }
        List<IUnit> Units { get; set; }
        void AddArea(IPopulatedArea area);
        void AddNeighbour(IZone zone);
        List<IZoneEntrance> ZoneEntrances { get; set; }
    }
}