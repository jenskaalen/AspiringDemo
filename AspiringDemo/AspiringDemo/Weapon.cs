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
        Staff
    }

    public class Weapon
    {
        [Key]
        public string WeaponName { get; set; }
        public int BaseDamage { get; set; }
        public int WeaponSpeed { get; set; }
    }
}
