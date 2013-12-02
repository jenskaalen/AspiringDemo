using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;

namespace AspiringDemo.Zones.Interiors
{
    public interface IInterior : IZone
    {
        List<Room> Rooms { get; set; }
        List<CorridorPath> Paths { get; set; }
        Room Entrance { get; set; }
        int InteriorWidth { get; set; }
        int InteriorHeight { get; set; }
        void Enter(IUnit unit);
        void CreateDebugImage();
    }
}

