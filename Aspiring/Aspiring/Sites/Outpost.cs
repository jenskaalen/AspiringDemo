using System;
using AspiringDemo.Factions;
using AspiringDemo.Zones;

namespace AspiringDemo.Sites
{
    [Serializable]
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