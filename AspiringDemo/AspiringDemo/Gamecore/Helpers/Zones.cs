using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore.Helpers
{
    public static class Zones
    {
        public static IZone GetClosestZone(Vector2 targetPosition, List<IZone> zones)
        {
            double lowestVal = int.MaxValue;
            IZone selectedZone = null;
            //search neighbours...
            foreach (var zone in zones)
            {
                double val = Math.Abs(targetPosition.X - zone.Position.X) + Math.Abs(targetPosition.Y - zone.Position.Y);

                if (val > lowestVal)
                    continue;

                selectedZone = (IZone)zone;
                lowestVal = val;
            }

            return selectedZone;
        }
    }
}
