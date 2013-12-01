using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.Zones;

namespace AspiringDemo
{
    public class Pathing
    {
        public List<IZone> Zones { get; set; }


        public IZone GetZone(int xPos, int yPos)
        {
            IZone zone = null;

            zone =
                Zones.FirstOrDefault(
                    xZone =>
                        xZone.Area.X1 < xPos && xZone.Area.X1 > xPos && xZone.Area.Y1 < yPos &&
                        xZone.Area.Y2 > yPos);

            return zone;
        }

        //public IZone GetZone(Vector2 pos)
        //{
        //    IZone zone = null;

        //    zone =
        //        Zones.FirstOrDefault(
        //            xZone =>
        //                xZone.Area.X1 < pos.X && xZone.X1 > pos.X && xZone.PositionYStart < pos.Y &&
        //                xZone.PositionYEnd > pos.Y);

        //    return zone;
        //}

        //public List<IZone> GetComputedZonePath(Vector2 startPosition, Vector2 endPosition)
        //{
        //    Pathfinder<IZone> finder = new Pathfinder<IZone>();
        //    finder.Nodes = Zonudes;

        //    //List<Zone> allZones = new List<Zone>();
        //    //Zonudes.ForEach(x => allZones.Add(x));

        //    //Zone startZone = GetZone(startPosition);
        //    //Zone endZone = GetZone(endPosition);

        //    List<IZone> computedList = new List<IZone>();

        //    //Zone currentZone = startZone;

        //    //while (currentZone != endZone)
        //    //{ 
        //    //    //currentZone.Neighbours.Min(zone => 
        //    //        //Math.Sqrt(zone.Area.X1 * zone.Area.X1 + endZone.Area.X1 * endZone.Area.X1))
        //    //    //if (currentZone.Neighbours == null
        //    //    //currentZone = allZones.Aggregate((z1, z2) => z1.Area.X1 > z2.Area.X1 ? z2 : z1);
        //    //    currentZone = currentZone.Neighbours.Aggregate((z1, z2) => z1.Area.X1 > z2.Area.X1 ? z1 : z2);
        //    //    allZones.Remove(currentZone);
        //    //    computedList.Add(currentZone);
        //    //}

        //    return computedList;
        //}
    }
}