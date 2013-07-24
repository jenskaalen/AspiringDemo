using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Pathfinding
    {
        public List<Zone> Zones { get; set; }

        public Zone GetZone(int xPos, int yPos)
        {
            Zone zone = null;

            zone = Zones.Where(x => x.PositionXStart < xPos && x.PositionXEnd > xPos && x.PositionYStart < yPos && x.PositionYEnd > yPos).FirstOrDefault();

            return zone;
        }
    }
}
