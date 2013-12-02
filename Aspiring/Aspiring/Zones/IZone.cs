using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Sites;

namespace AspiringDemo.Zones
{
    public interface IZone : IPathfindingNode
    {
        List<IPathfindingNode> Nodes { get; set; }
        bool IsPlayerNearby { get; set; }
        List<IPopulatedArea> PopulatedAreas { get; set; }
        Rect Area { get; set; }
        ZoneType Type { get; set; }
        List<IUnit> Units { get; set; }
        void AddArea(IPopulatedArea area);
        void AddNeighbour(IZone zone);
        List<IZoneEntrance> ZoneEntrances { get; set; }
        void AddEntrance(IZone entrance, Vector2 positionVector2);
        Pathfinder<IPathfindingNode> Pathfinder { get; set; }
}
}