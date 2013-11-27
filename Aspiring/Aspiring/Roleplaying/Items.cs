using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Roleplaying
{
    public class Items : IItems
    {
        public IWeapon CurrentWeapon { get; private set; }
        public List<IWeapon> Weapons { get; set; }

        public IWeapon GetBestWeapon()
        {
            IWeapon bestWeapon = Weapons.Aggregate((seed, f) => f.BaseDamage > seed.BaseDamage ? f : seed);
            return bestWeapon;
        }

        public void EquipWeapon(IWeapon weapon)
        {
            CurrentWeapon = weapon;
        }
    }
}
