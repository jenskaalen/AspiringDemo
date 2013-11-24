using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying.Stats
{
    public interface IWeaponStats
    {
        int Onehanded { get; set; }
        int Twohanded { get; set; }
        int Bows { get; set; }
    }
}
