using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Weapons
{
    public class Smackhammer : IWeapon
    {
        public WeaponType Type { get; set; }
        public WieldType Wielding { get; set; }
        public int BaseDamage { get; set; }
        public int ID { get; set; }
        public string WeaponName { get; set; }
        public int WeaponSpeed { get; set; }

        public Smackhammer() 
        {
            this.BaseDamage = 200;
            this.Type = WeaponType.Staff;
        }
    }
}
