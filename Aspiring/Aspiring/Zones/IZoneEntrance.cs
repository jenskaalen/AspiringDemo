using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Zones
{
    public interface IZoneEntrance
    {
        Vector2 Position { get; set; } 
        IZone Zone { get; set; }
    }
}
