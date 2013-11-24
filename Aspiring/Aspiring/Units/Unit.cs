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
    public sealed class Unit : BaseUnit, IUnitRoleplayable
    {
        public ICharacterStats Stats { get; set; }
        public ICharacterSkills Skills { get; set; }
        public ICharacterLevel CharacterLevel { get; set; }

        public override int Hp
        {
            get
            {
                return Stats.CurrentHp;
            }
            set
            {
                if (State == UnitState.Dead || State == UnitState.ObjectDisabled)
                    return;

                if (value < 1)
                {
                    Die();
                }

                Stats.CurrentHp = value;
            }
        }

        //public override UnitState State
        //{
        //    get
        //    {
        //        return _state;
        //    }
        //    set
        //    {
        //        ChangeState(this, value);

        //        if (Squad != null)
        //            Squad.MemberChangedState(this, value);
        //        //throw new NotImplementedException();
        //    }
        //}

        //private SquadRank _rank;
        //private UnitState _state;
        //private long _objectDestructionTime;

        public Unit(IFaction faction) : base(faction)
        {
            Stats = new CharacterStats();
            Speed = 20;
            Hp = 25;
            XPWorth = 50;
            CharacterLevel = new CharacterLevel(new LevelProgressModifier());
            CharacterLevel.GainLevel += Stats.GainLevel;
            Name = "Soldier";
        }

        public override void TimeTick(long time)
        {
            if (State == UnitState.Dead)
            {
                if (ObjectDestructionTime < time)
                    Remove();
                return;
            }

            Stats.Regen(time);
        }

        public override void KilledUnit(IUnit target)
        {
            Kills++;

            CharacterLevel.GainXP(target.XPWorth);
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
