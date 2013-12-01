using AspiringDemo.Factions;
using AspiringDemo.Zones;

namespace AspiringDemo.Sites
{
    public class Outpost : PopulatedArea
    {
        public Outpost(IFaction owner, IZone zone) : base(owner, zone)
        {
        }


        //public Outpost(IFaction owner) : base(owner)
        //{
        //}
    }
}