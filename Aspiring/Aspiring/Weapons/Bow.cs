namespace AspiringDemo.Weapons
{
    public class Bow : IWeapon
    {
        public Bow()
        {
            Type = WeaponType.Bow;
            Wielding = WieldType.TwoHanded;
            BaseDamage = 10;
            WeaponSpeed = 10;
        }

        public WeaponType Type { get; set; }

        public WieldType Wielding { get; set; }

        public int BaseDamage { get; set; }

        public int ID { get; set; }

        public string WeaponName { get; set; }

        public int WeaponSpeed { get; set; }
    }
}