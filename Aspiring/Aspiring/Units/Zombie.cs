using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Orders;
using AspiringDemo.Weapons;

namespace AspiringDemo.Units
{
    public class Zombie : BaseUnit
    {
        public Zombie(IFaction faction) : base(faction)
        {
            Name = "Zombie";
            Speed = 20;
            Hp = 20;
            Weapons = new List<IWeapon>();
            Weapons.Add(new Unarmed());
            XPWorth = 50;
            Faction = faction;

            //Stats = new UnitStats();
            //ApplyAction += ApplyActionOnSelf;
            //ChangeState += ChangeStateSelf;
            //Rank = SquadRank.Private;
            //Name = "";
            //CharacterLevel = new CharacterLevel(new LevelProgressModifier());
            //CharacterLevel.GainLevel += Stats.GainLevel;
            //Name = "Soldier";
        }


        public IWeapon EquippedWeapon { get; set; }

        public IFaction Faction { get; private set; }
        
        public int Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                if (value < 1)
                {
                    Remove();
                }

                _hp = value;
            }
        }

        private int _hp = 0;

        public int ID { get; set; }

        public bool IsPlayer { get; set; }
        
        public int Kills { get; private set; }

        public string Name { get; set; }

        public Orders.IUnitOrder Order { get; set; }

        public SquadRank Rank { get; set; }

        public int Speed { get; set; }

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

        public IWeapon SelectBestWeapon()
        {
            if (Weapons == null)
                throw new Exception("Weapons must be worn! (no weapons to attack with on unit)");

            IWeapon weapon = Weapons.OrderByDescending(x => x.BaseDamage).FirstOrDefault();
            EquippedWeapon = weapon;

            return weapon;
        }
        public int GetDamageOutput()
        {
            SelectBestWeapon();

            int dmg = EquippedWeapon.BaseDamage;

            return dmg;
        }


        public void KilledUnit(IUnit target)
        {
            Kills++;

            // create a zombie!
            var unit = Faction.CreateUnit();
            unit.EnterZone(Zone);
        }

        private void Remove()
        {
            // cleanup
            State = UnitState.Dead;
            Faction.Army.Units.Remove(this);
            Zone.Units.Remove(this);

            if (Squad != null)
                Squad.RemoveMember(this);
        }
    }
}
