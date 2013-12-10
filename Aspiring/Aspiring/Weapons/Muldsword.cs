using System;

namespace AspiringDemo.Weapons
{
    [Serializable]
    public class Muldsword : IWeapon
    {
        public Muldsword()
        {
            BaseDamage = 15;
        }

        public WeaponType Type { get; set; }

        public WieldType Wielding { get; set; }

        public int BaseDamage { get; set; }

        public int ID { get; set; }

        public string WeaponName { get; set; }

        public int WeaponSpeed { get; set; }
    }
}