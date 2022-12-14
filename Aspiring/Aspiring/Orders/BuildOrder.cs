using System;
using AspiringDemo.Sites;
using AspiringDemo.Zones;

namespace AspiringDemo.Orders
{
    [Serializable]
    public class BuildOrder
    {
        public BuildOrder(Zone buildLocation, IPopulatedArea area)
        {
        }

        public IZone BuildLocation { get; private set; }
        public IPopulatedArea BuildType { get; private set; }
    }
}