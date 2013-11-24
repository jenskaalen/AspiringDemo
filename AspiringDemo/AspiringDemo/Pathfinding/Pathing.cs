using AspiringDemo.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Pathing
    {
        public List<IZone> Zones { get; set; }


        public IZone GetZone(int xPos, int yPos)
        {
            IZone zone = null;

            zone = Zones.FirstOrDefault(xZone => xZone.PositionXStart < xPos && xZone.PositionXEnd > xPos && xZone.PositionYStart < yPos && xZone.PositionYEnd > yPos);

            return zone;
        }

        public IZone GetZone(Vector2 pos)
        {
            IZone zone = null;

            zone = Zones.FirstOrDefault(xZone => xZone.PositionXStart < pos.X && xZone.PositionXEnd > pos.X && xZone.PositionYStart < pos.Y && xZone.PositionYEnd > pos.Y);

            return zone;
        }

        //public List<IZone> GetComputedZonePath(Vector2 startPosition, Vector2 endPosition)
        //{
        //    Pathfinder<IZone> finder = new Pathfinder<IZone>();
        //    finder.Nodes = Zones;

        //    //List<Zone> allZones = new List<Zone>();
        //    //Zones.ForEach(x => allZones.Add(x));

        //    //Zone startZone = GetZone(startPosition);
        //    //Zone endZone = GetZone(endPosition);

        //    List<IZone> computedList = new List<IZone>();

        //    //Zone currentZone = startZone;

        //    //while (currentZone != endZone)
        //    //{ 
        //    //    //currentZone.Neighbours.Min(zone => 
        //    //        //Math.Sqrt(zone.PositionXStart * zone.PositionXStart + endZone.PositionXStart * endZone.PositionXStart))
        //    //    //if (currentZone.Neighbours == null
        //    //    //currentZone = allZones.Aggregate((z1, z2) => z1.PositionXStart > z2.PositionXStart ? z2 : z1);
        //    //    currentZone = currentZone.Neighbours.Aggregate((z1, z2) => z1.PositionXStart > z2.PositionXStart ? z1 : z2);
        //    //    allZones.Remove(currentZone);
        //    //    computedList.Add(currentZone);
        //    //}

        //    return computedList;
        //}
        

    }
}
