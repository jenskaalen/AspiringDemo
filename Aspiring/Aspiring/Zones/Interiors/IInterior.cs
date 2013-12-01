using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Zones.Interiors
{
    public interface IInterior
    {
        List<IInteriorNode> InteriorNodes { get; set; }
        List<Room> Rooms { get; set; }
        List<CorridorPath> Paths { get; set; }
        Room Entrance { get; set; }
        int InteriorWidth { get; set; }
        int InteriorHeight { get; set; }
        void Enter(IUnit unit);
        List<IUnit> Units { get; }
        void CreateDebugImage();
    }
}

