using System;
using System.Collections.Generic;
using System.Linq;

namespace AspiringDemo.Roleplaying
{
    [Serializable]
    public class Items : IItems
    {
        public Items()
        {
            Weapons = new List<IWeapon>();
        }

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