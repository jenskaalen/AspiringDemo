using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public enum WeaponType
    {
        Sword,
        Bow,
        Staff,
        Unarmed
    }

    public enum WieldType
    { 
        OneHanded,
        TwoHanded
    }

    //public class Weapon : AspiringDemo.IWeapon
    //{
    //    [Key]
    //    public WeaponType Type { get; set; }
    //    public WieldType Wielding { get; set; }
    //    public int ID { get; set; }
    //    public string WeaponName { get; set; }
    //    public int BaseDamage { get; set; }
    //    public int WeaponSpeed { get; set; }
    //}
}
