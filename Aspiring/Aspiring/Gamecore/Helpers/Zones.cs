using System;
using System.Collections.Generic;

namespace AspiringDemo.Gamecore.Helpers
{
    public static class Zones
    {
        public static IZone GetClosestZone(Vector2 targetPosition, List<IZone> zones)
        {
            double lowestVal = int.MaxValue;
            IZone selectedZone = null;
            //search neighbours...
            foreach (IZone zone in zones)
            {
                double val = Math.Abs(targetPosition.X - zone.Position.X) + Math.Abs(targetPosition.Y - zone.Position.Y);

                if (val > lowestVal)
                    continue;

                selectedZone = zone;
                lowestVal = val;
            }

            return selectedZone;
        }
    }
}