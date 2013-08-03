using AspiringDemo.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    // is supposed to run in its own thread
    public class OrderHandler
    {
        public List<ICharacterOrder> Orders { get; set; }
        public bool Running { get; set; }

        public void ProcessOrders()
        {
            Running = true;

            while (Running)
            {
                foreach (ICharacterOrder order in Orders)
                { 
                }
            }
        }
    }
}
