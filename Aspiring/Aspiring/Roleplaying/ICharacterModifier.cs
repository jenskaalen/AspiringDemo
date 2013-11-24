using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Units;

namespace AspiringDemo.Roleplaying
{
    interface ICharacterModifier
    {
        void Modify(IUnit unit);
    }
}
