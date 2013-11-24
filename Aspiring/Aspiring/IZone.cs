using AspiringDemo.Combat;
using AspiringDemo.Pathfinding;
using System;
using AspiringDemo.Units;

namespace AspiringDemo
{
    public interface IZone : IPathfindingNode
    {
        void AddArea(AspiringDemo.Sites.IPopulatedArea area);
        void AddNeighbour(IZone zone);
        void EnterZone(ISquad squad);
        void EnterZone(IUnit unit);
        Fight Fight { get; set; }
        int ID { get; set; }
        bool IsPlayerNearby { get; set; }
        void LeaveZone(IUnit unit);
        System.Collections.Generic.List<AspiringDemo.Sites.IPopulatedArea> PopulatedAreas { get; set; }
        int PositionXEnd { get; set; }
        int PositionXStart { get; set; }
        int PositionYEnd { get; set; }
        int PositionYStart { get; set; }
        ZoneType Type { get; set; }
        System.Collections.Generic.List<IUnit> Units { get; set; }
    }
}
