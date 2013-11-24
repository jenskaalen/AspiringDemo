using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;

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
