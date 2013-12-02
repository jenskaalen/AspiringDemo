using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameActions;
using AspiringDemo.Gamecore;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.Orders;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
using AspiringDemo.Zones.Interiors;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo.GameObjects.Units
{
    //TODO: Extract interface
    public class BaseUnit : IUnit
    {
        protected float ObjectDestructionTime;
        protected int _hp;
        protected SquadRank _rank;
        protected UnitState _state;

        protected BaseUnit(IFaction faction)
        {
            Faction = faction;
            ChangeState += ChangeStateSelf;
            Rank = SquadRank.Private;
            State = UnitState.Idle;
            Stats = GameFrame.Game.Factory.Get<IUnitStats>();
            Items = GameFrame.Game.Factory.Get<IItems>();
            CombatModule = GameFrame.Game.Factory.Get<ICombatModule>(new ConstructorArgument("unit", this));
            Stats.Speed = 20;
            _hp = 25;
            XPWorth = 50;
            //Weapons = new List<IWeapon>();
            //Weapons.Add(new Unarmed());
            Items.Weapons.Add(new Unarmed());
            Name = "Soldier";
            Actions = new List<GameAction>();
        }

        public StateChanged ChangeState { get; set; }

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
        public IActionProcesser ActionProcesser { get; set; }
        public IItems Items { get; set; }
        public List<GameAction> Actions { get; set; }
        public IFaction Faction { get; protected set; }
        public ICombatModule CombatModule { get; set; }
        public RankChanged ChangeRank { get; set; }
        public IUnitOrder Order { get; set; }
        public Vector2 Position { get; set; }
        public IZone Zone { get; set; }
        public IUnitStats Stats { get; set; }
        public IInterior Interior { get; set; }

        //TODO: Possibly rework this..
        public int ID { get; set; }

        public virtual int Hp
        {
            get { return Stats.CurrentHp; }
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

        public virtual UnitState State
        {
            get { return _state; }
            set
            {
                ChangeState(this, value);

                if (Squad != null)
                    Squad.MemberChangedState(this, value);
                //throw new NotImplementedException();
            }
        }

        public void EnterZone(IZone zone)
        {
            if (Zone != null)
                LeaveZone();

            Zone = zone;

            if (zone is IInterior)
                Position = ((IInterior) (zone)).Entrance.Center;
            else
                Position = zone.Position;


            Zone.Units.Add(this);

            if (State == UnitState.Dead)
                return;
            //unit enters combat..
            IUnit enemyUnit = Zone.Units.Where(unit => unit.State != UnitState.Dead).FirstOrDefault(
                unit => unit.Faction.Relations.GetRelation(Faction).Relation == RelationType.Hostile);

            if (enemyUnit != null)
            {
                INewFight fight = enemyUnit.CombatModule.CurrentFight;

                if (fight == null)
                {
                    fight = GameFrame.Game.Factory.Get<INewFight>();
                    fight.Enter(enemyUnit);
                }

                fight.Enter(this);
            }
        }

        public void LeaveZone()
        {
            Zone.Units.Remove(this);
            Zone = null;
        }

        public virtual void AssignOrder(IUnitOrder order)
        {
            Order = order;
        }

        public virtual void TimeTick(float time)
        {
            if (State == UnitState.Dead)
            {
                if (ObjectDestructionTime < time)
                    Remove();
                return;
            }

            Actions.ForEach(a => a.Update(time));
            Actions.RemoveAll(a => a.Finished);

            Stats.Regen(time);
        }

        public virtual void KilledUnit(IUnit unit)
        {
            CombatModule.Kills++;
        }

        public void EnterInterior(IInterior interior)
        {
            Zone = null;
            Interior = interior;
            Position = interior.Entrance.Center;
            interior.Units.Add(this);
        }


        public void LeaveInterior()
        {
            Interior.Units.Remove(this);
            Interior = null;
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

        protected virtual void Die()
        {
            // cleanup
            State = UnitState.Dead;

            // remove from any fights
            if (CombatModule.CurrentFight != null)
                CombatModule.CurrentFight.Leave(this);

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
                int highestBasedmg = Items.Weapons.Max(wpn => wpn.BaseDamage);
                IEnumerable<IWeapon> lootable = unit.Items.Weapons.Where(weapon => weapon.BaseDamage > highestBasedmg);
                var toBeRemoved = new List<IWeapon>();

                foreach (IWeapon weapon in lootable)
                {
                    Items.Weapons.Add(weapon);
                    toBeRemoved.Add(weapon);
                }

                toBeRemoved.ForEach(weapon => unit.Items.Weapons.Remove(weapon));
            }
            else
            {
                GameFrame.Debug.Log(
                    String.Format("Can't loot a unit that is not dead (this was not supposed to happen): {0}",
                        unit.GetHashCode()));
            }
        }
    }
}