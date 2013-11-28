using AspiringDemo.Sites;

namespace AspiringDemo.Orders
{
    public class BuildOrder
    {
        public BuildOrder(Zone buildLocation, IPopulatedArea area)
        {
        }

        public IZone BuildLocation { get; private set; }
        public IPopulatedArea BuildType { get; private set; }
    }
}