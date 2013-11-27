using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Combat
{
    public interface INewFight
    {
        List<IUnit> Units { get; set; }
        bool Finished { get; set; }
        void Leave(IUnit unit);
        void Enter(IUnit unit);
    }

}
