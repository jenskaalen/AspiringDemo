
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Procedural.Interiors
{
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
