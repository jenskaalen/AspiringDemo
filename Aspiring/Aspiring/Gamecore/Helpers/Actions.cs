using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Orders;
using AspiringDemo.Units;
using AspiringDemo.Factions;
using AspiringDemo.Combat;

namespace AspiringDemo.Gamecore.Helpers
{
    public static class Actions
    {
        public static void GiveRetreatOrder(IUnit unit, IZone retreatZone)
        {
            TravelOrder.GiveTravelOrder(unit, retreatZone, false);
            unit.Order.Execute();
            unit.Order.Update(GameFrame.Game.GameTime.Time);
        }

        public static void GiveRetreatOrder(IFaction faction, IFight fight, IZone retreatZone)
        {
            foreach (var unit in fight.FightingUnits.Where(un => un.Faction == faction))
            {
                TravelOrder.GiveTravelOrder(unit, retreatZone, false);

                if (unit.Order == null)
                {
                    GameFrame.Debug.Log(String.Format("Unit can't retreat because order is null: {0} , {1}, {2}", unit.Faction, unit.State, unit.GetHashCode()));
                    return;
                }
                unit.Order.Execute();
                unit.Order.Update(GameFrame.Game.GameTime.Time);
            }
        }
    }
}
