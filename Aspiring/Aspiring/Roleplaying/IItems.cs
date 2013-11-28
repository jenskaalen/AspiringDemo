using System.Collections.Generic;

namespace AspiringDemo.Roleplaying
{
    public interface IItems
    {
        IWeapon CurrentWeapon { get; }
        List<IWeapon> Weapons { get; set; }
        IWeapon GetBestWeapon();
        void EquipWeapon(IWeapon weapon);
    }
}