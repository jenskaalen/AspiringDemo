using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.Orders;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Weapons;

namespace AspiringDemo.Units
{
    public enum UnitState
    {
        Idle,
        Fighting,
        ExecutingOrder,
        Dead,
        Waiting
    }

    public delegate ActionResult ActionApplied(Action action);
    public delegate void StateChanged(IUnit unit, UnitState state);
    public delegate void RankChanged(IUnit unit, SquadRank rank);

    //TODO: Extract interface
    public class Unit : IUnitRoleplayable
    {
        public SquadRank Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;

                if (ChangeRank != null)
                    ChangeRank(this, value);
            }
        }

        public string Name { get; set; }
        public bool IsPlayer { get; set; }
        public int XPWorth { get; set; }
        public ISquad Squad { get; set; }
        public IFaction Faction { get; private set; }
        public ICharacterStats Stats { get; set; }
        public IWeaponStats WeaponStats { get; set; }
        public ICharacterSkills Skills { get; set; }
        public ICharacterLevel CharacterLevel { get; set; }

        public IWeapon EquippedWeapon { get; set; }
        public List<IWeapon> Weapons { get; set; }

        //TODO: Possibly rework this..
        public int ID { get; set; }

        //public int Hp { get; set; }
        public int Hp
        {
            get
            {
                return Stats.CurrentHp;
            }
            set
            {
                if (value < 1)
                {
                    Remove();
                }

                Stats.CurrentHp = value;
            }
        }

        public int Speed { get; set; }
        public int Toughness { get; set; }
        public int Kills { get; private set; }

        public RankChanged ChangeRank { get; set; }
        public StateChanged ChangeState { get; set; }
        public IUnitOrder Order { get; set; }
        public IZone Zone { get; set; }

        public UnitState State
        {
            get
            {
                return _state;
            }
            set
            {
                ChangeState(this, value);

                if (Squad != null)
                    Squad.MemberChangedState(this, value);
                //throw new NotImplementedException();
            }
        }

        private SquadRank _rank;
        private UnitState _state;

        public Unit(IFaction faction)
        {
            Faction = faction;
            Stats = new CharacterStats();
            ChangeState += ChangeStateSelf;
            Rank = SquadRank.Private;
            State = UnitState.Idle;
            Speed = 20;
            Hp = 25;
            XPWorth = 50;
            Weapons = new List<IWeapon>();
            Weapons.Add(new Unarmed());
            CharacterLevel = new CharacterLevel(new LevelProgressModifier());
            CharacterLevel.GainLevel += Stats.GainLevel;
            Name = "Soldier";
        }

        //public virtual void Attack(IUnit target)
        //{
        //    IWeapon weapon = SelectBestWeapon();
        //    Action action = new Action();
        //    action.HPModifier = -weapon.BaseDamage;

        //    if (Stats != null)
        //        action.HPModifier -= Stats.Strength;

        //    //TODO: Refactor so that an attack can hit multiple targets? (Or use different attack for that perhaps)
        //    ActionResult result = target.ApplyAction(action);

        //    if (result.KilledTarget)
        //    {
        //        Kills++;

        //        //TODO: Redo this to something more clevererer
        //        if (CharacterLevel != null)
        //            CharacterLevel.GainXP(target.XPWorth);
        //    }
        //}

        public IWeapon SelectBestWeapon()
        {
            if (Weapons == null)
                throw new Exception("Weapons must be worn! (no weapons to attack with on unit)");

            IWeapon weapon = Weapons.OrderByDescending(x => x.BaseDamage).FirstOrDefault();
            EquippedWeapon = weapon;

            return weapon;
        }


        private void ChangeStateSelf(IUnit unit, UnitState state)
        {
            // we dont want to change the state of a dead unit
            if (State == UnitState.Dead)
                return;

            if (state == UnitState.Idle && unit.Order != null)
                _state = UnitState.ExecutingOrder;
            else
                _state = state;
        }

        public void AssignOrder(IUnitOrder order)
        {
            Order = order;
        }


        public void TimeTick(long time)
        {
            Stats.Regen(time);
        }


        public int GetDamageOutput()
        {
            SelectBestWeapon();

            int dmg = EquippedWeapon.BaseDamage;
            dmg += Stats.Strength;

            return dmg;
        }


        public void KilledUnit(IUnit target)
        {
            Kills++;

            CharacterLevel.GainXP(target.XPWorth);
        }

        private void Remove()
        {
            // cleanup
            State = UnitState.Dead;
            Faction.Army.Units.Remove(this);

            if (Zone != null)
                Zone.Units.Remove(this);
            
            if (Squad != null)
                Squad.RemoveMember(this);
        }
    }
}
