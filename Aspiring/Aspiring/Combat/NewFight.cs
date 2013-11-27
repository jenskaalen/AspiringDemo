using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Combat
{
    public class NewFight : INewFight
    {
        public List<IUnit> Units { get; set; }
        public bool Finished { get; set; }
    }
}
