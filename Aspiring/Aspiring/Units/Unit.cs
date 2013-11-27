using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.Gamecore;
using AspiringDemo.Orders;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Weapons;

namespace AspiringDemo.Units
{

    //TODO: Extract interface
    public sealed class Unit : BaseUnit, IUnitLeveling
    {
        public ICharacterLevel CharacterLevel { get; set; }

        public Unit(IFaction faction) : base(faction)
        {
            Stats.Speed = 20;
            Hp = 25;
            XPWorth = 50;
            CharacterLevel = new CharacterLevel(new LevelProgressModifier());
            CharacterLevel.GainLevel += Stats.GainLevel;
            Name = "Soldier";
        }

        //public override void KilledUnit(IUnit target)
        //{
        //    Kills++;

        //    int bestweapon = Items.Weapons.Max(x => x.BaseDamage);

        //    foreach (var weapon in target.Items.Weapons.Where(wpn => wpn.BaseDamage > bestweapon))
        //    {
        //        Items.Weapons.Add(weapon);
        //    }

        //    target.Items.Weapons.RemoveAll(wpn => wpn.BaseDamage > bestweapon);
        //    CharacterLevel.GainXP(target.XPWorth);
        //}

        public override void KilledUnit(IUnit unit)
        {
            base.KilledUnit(unit);
            Loot(unit);
            CharacterLevel.GainXP(unit.XPWorth);
        }

        //private void Remove()
        //{
        //    Faction.Army.Units.Remove(this);

        //    if (Zone != null)
        //        Zone.Units.Remove(this);

        //    if (Squad != null)
        //        Squad.RemoveMember(this);
        //}

        //public void Loot(IUnit unit)
        //{
        //    if (unit.State == UnitState.Dead)
        //    {
        //        int highestBasedmg = Weapons.Max(wpn => wpn.BaseDamage);
        //        var lootable = unit.Weapons.Where(weapon => weapon.BaseDamage > highestBasedmg);

        //        foreach (var weapon in lootable)
        //        {
        //            Weapons.Add(weapon);
        //        }
        //    }
        //    else
        //    {
        //        GameFrame.Debug.Log(String.Format("Can't loot a unit that is not dead (this was not supposed to happen): {0}", unit.GetHashCode()));
        //    }
        //}
    }
}
