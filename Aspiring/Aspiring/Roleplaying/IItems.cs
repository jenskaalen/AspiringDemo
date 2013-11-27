using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying
{
    public interface IItems
    {
        IWeapon CurrentWeapon { get;  }
        List<IWeapon> Weapons { get; set; }
        IWeapon GetBestWeapon();
        void EquipWeapon(IWeapon weapon);
    }
}
