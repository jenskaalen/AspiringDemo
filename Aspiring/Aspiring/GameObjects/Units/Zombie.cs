using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Weapons;

namespace AspiringDemo.GameObjects.Units
{
    [Serializable]
    public class Zombie : BaseUnit
    {
        public Zombie(IFaction faction)
            : base(faction)
        {
            Name = "Zombie";
            Stats.Speed = 20;
            Hp = 20;
            XPWorth = 50;
            Faction = faction;
        }
    }
}
