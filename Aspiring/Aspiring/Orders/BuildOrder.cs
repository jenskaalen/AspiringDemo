using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Orders
{
    public class BuildOrder
    {
        public IZone BuildLocation { get; private set; }
        public IPopulatedArea BuildType { get; private set; }


        public BuildOrder(Zone buildLocation, IPopulatedArea area)
        { 
            
        }
    }
}
