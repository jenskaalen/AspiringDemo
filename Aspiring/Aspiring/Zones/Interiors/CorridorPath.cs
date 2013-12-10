using System;
using System.Collections.Generic;

namespace AspiringDemo.Zones.Interiors
{
    [Serializable]
    public class CorridorPath
    {
        public List<Corridor> Corridors { get; set; }
        public List<Room> ConnectedRooms { get; set; }
        public string PathType { get; set; }

        public CorridorPath()
        {
            Corridors = new List<Corridor>();
            ConnectedRooms = new List<Room>();
        }
    }
}
