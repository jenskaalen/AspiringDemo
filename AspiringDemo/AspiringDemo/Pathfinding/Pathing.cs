using AspiringDemo.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Pathing
    {
        public List<Zone> Zones { get; set; }


        public Zone GetZone(int xPos, int yPos)
        {
            Zone zone = null;

            zone = Zones.Where(xZone => xZone.PositionXStart < xPos && xZone.PositionXEnd > xPos && xZone.PositionYStart < yPos && xZone.PositionYEnd > yPos).FirstOrDefault();

            return zone;
        }

        public Zone GetZone(Vector2 pos)
        {
            Zone zone = null;

            zone = Zones.Where(xZone => xZone.PositionXStart < pos.X && xZone.PositionXEnd > pos.X && xZone.PositionYStart < pos.Y && xZone.PositionYEnd > pos.Y).FirstOrDefault();

            return zone;
        }

        public List<Zone> GetComputedZonePath(Vector2 startPosition, Vector2 endPosition)
        {
            Pathfinder<Zone> finder = new Pathfinder<Zone>();
            finder.Nodes = Zones;

            //List<Zone> allZones = new List<Zone>();
            //Zones.ForEach(x => allZones.Add(x));

            //Zone startZone = GetZone(startPosition);
            //Zone endZone = GetZone(endPosition);

            List<Zone> computedList = new List<Zone>();

            //Zone currentZone = startZone;

            //while (currentZone != endZone)
            //{ 
            //    //currentZone.Neighbours.Min(zone => 
            //        //Math.Sqrt(zone.PositionXStart * zone.PositionXStart + endZone.PositionXStart * endZone.PositionXStart))
            //    //if (currentZone.Neighbours == null
            //    //currentZone = allZones.Aggregate((z1, z2) => z1.PositionXStart > z2.PositionXStart ? z2 : z1);
            //    currentZone = currentZone.Neighbours.Aggregate((z1, z2) => z1.PositionXStart > z2.PositionXStart ? z1 : z2);
            //    allZones.Remove(currentZone);
            //    computedList.Add(currentZone);
            //}

            return computedList;
        }
        

    }
}
