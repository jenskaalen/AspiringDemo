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
    public delegate ActionResult ActionApplied(Action action);
    public delegate void StateChanged(IUnit unit, UnitState state);
    public delegate void RankChanged(IUnit unit, SquadRank rank);

    //TODO: Extract interface
    public class BaseUnit : IUnit
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
        public IWeaponStats WeaponStats { get; set; }

        public IWeapon EquippedWeapon { get; set; }
        public List<IWeapon> Weapons { get; set; }

        //TODO: Possibly rework this..
        public int ID { get; set; }

        //public int Hp { get; set; }
        public virtual int Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                if (value < 1)
                {
                    Die();
                }

                _hp = value;
            }
        }

        public int Speed { get; set; }
        public int Toughness { get; set; }
        public int Kills { get; protected set; }

        public RankChanged ChangeRank { get; set; }
        public StateChanged ChangeState { get; set; }
        public IUnitOrder Order { get; set; }
        public IZone Zone { get; set; }

        public virtual UnitState State
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
        protected long ObjectDestructionTime;
        private int _hp;

        public BaseUnit(IFaction faction)
        {
            Faction = faction;
            ChangeState += ChangeStateSelf;
            Rank = SquadRank.Private;
            State = UnitState.Idle;
            Speed = 20;
            _hp = 25;
            XPWorth = 50;
            Weapons = new List<IWeapon>();
            Weapons.Add(new Unarmed());
            Name = "Soldier";
        }

        public virtual IWeapon SelectBestWeapon()
        {
            if (Weapons == null)
                throw new Exception("Weapons must be worn! (no weapons to attack with on unit)");

            IWeapon weapon = Weapons.OrderByDescending(x => x.BaseDamage).FirstOrDefault();
            EquippedWeapon = weapon;

            return weapon;
        }


        protected virtual void ChangeStateSelf(IUnit unit, UnitState state)
        {
            // we dont want to change the state of a dead unit
            if (State == UnitState.Dead || State == UnitState.ObjectDisabled)
                return;

            if (state == UnitState.Idle && unit.Order != null)
                _state = UnitState.ExecutingOrder;
            else
                _state = state;
        }

        public virtual void AssignOrder(IUnitOrder order)
        {
            Order = order;
        }

        public virtual void TimeTick(long time)
        {
            if (State == UnitState.Dead)
            {
                if (ObjectDestructionTime < time)
                    Remove();
                return;
            }
        }


        public int GetDamageOutput()
        {
            SelectBestWeapon();
            int dmg = EquippedWeapon.BaseDamage;

            return dmg;
        }


        public virtual void KilledUnit(IUnit target)
        {
            Kills++;
        }

        protected virtual void Die()
        {
            // cleanup
            State = UnitState.Dead;
            ObjectDestructionTime = GameFrame.Game.GameTime.Time + 60;
        }

        protected virtual void Remove()
        {
            Faction.Army.Units.Remove(this);

            if (Zone != null)
                Zone.Units.Remove(this);

            if (Squad != null)
                Squad.RemoveMember(this);
        }

        public virtual void Loot(IUnit unit)
        {
            if (unit.State == UnitState.Dead)
            {
                int highestBasedmg = Weapons.Max(wpn => wpn.BaseDamage);
                var lootable = unit.Weapons.Where(weapon => weapon.BaseDamage > highestBasedmg);

                foreach (var weapon in lootable)
                {
                    Weapons.Add(weapon);
                }
            }
            else
            {
                GameFrame.Debug.Log(String.Format("Can't loot a unit that is not dead (this was not supposed to happen): {0}", unit.GetHashCode()));
            }
        }
    }
}
