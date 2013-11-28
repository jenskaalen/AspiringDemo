using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Sites;

namespace AspiringDemo
{
    public interface IZone : IPathfindingNode
    {
        //TODO: cleanup
        //void EnterZone(ISquad squad);
        //void EnterZone(IUnit unit);
        //void LeaveZone(IUnit unit);
        //Fight Fight { get; set; }
        int ID { get; set; }
        bool IsPlayerNearby { get; set; }
        List<IPopulatedArea> PopulatedAreas { get; set; }
        int PositionXEnd { get; set; }
        int PositionXStart { get; set; }
        int PositionYEnd { get; set; }
        int PositionYStart { get; set; }
        ZoneType Type { get; set; }
        List<IUnit> Units { get; set; }
        void AddArea(IPopulatedArea area);
        void AddNeighbour(IZone zone);
    }
}