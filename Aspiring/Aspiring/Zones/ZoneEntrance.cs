using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Zones
{
    public class ZoneEntrance : IZoneEntrance
    {
        public Vector2 Position { get; set; }
        public IZone Zone { get; set; }

        public ZoneEntrance(Vector2 position, IZone zone)
        {
            Position = position;
            Zone = zone;
        }
    }
}
