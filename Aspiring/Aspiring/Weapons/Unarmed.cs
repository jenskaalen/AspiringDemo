using System;

namespace AspiringDemo.Weapons
{
    [Serializable]
    public class Unarmed : IWeapon
    {
        public Unarmed()
        {
            Type = WeaponType.Unarmed;
            Wielding = WieldType.TwoHanded;
            BaseDamage = 2;
            WeaponSpeed = 15;
        }

        public WeaponType Type { get; set; }

        public WieldType Wielding { get; set; }

        public int BaseDamage { get; set; }

        public int ID { get; set; }

        public string WeaponName { get; set; }

        public int WeaponSpeed { get; set; }
    }
}