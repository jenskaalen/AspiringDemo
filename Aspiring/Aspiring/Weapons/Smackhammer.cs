namespace AspiringDemo.Weapons
{
    public class Smackhammer : IWeapon
    {
        public Smackhammer()
        {
            BaseDamage = 200;
            Type = WeaponType.Staff;
        }

        public WeaponType Type { get; set; }
        public WieldType Wielding { get; set; }
        public int BaseDamage { get; set; }
        public int ID { get; set; }
        public string WeaponName { get; set; }
        public int WeaponSpeed { get; set; }
    }
}