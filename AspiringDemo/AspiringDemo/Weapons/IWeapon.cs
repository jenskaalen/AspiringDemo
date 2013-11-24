using System;
namespace AspiringDemo
{
    public interface IWeapon
    {
        WeaponType Type { get; set; }
        WieldType Wielding { get; set; }
        int BaseDamage { get; set; }
        int ID { get; set; }
        string WeaponName { get; set; }
        int WeaponSpeed { get; set; }
    }
}
