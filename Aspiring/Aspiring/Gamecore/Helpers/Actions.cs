using System;
using System.Linq;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Zones;

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

        public static void GiveRetreatOrder(IFaction faction, INewFight fight, IZone retreatZone)
        {
            foreach (IUnit unit in fight.Units.Where(un => un.Faction == faction))
            {
                unit.CombatModule.CurrentFight.Leave(unit);

                TravelOrder.GiveTravelOrder(unit, retreatZone, false);

                if (unit.Order == null)
                {
                    GameFrame.Debug.Log(String.Format("Unit can't retreat because order is null: {0} , {1}, {2}",
                        unit.Faction, unit.State, unit.GetHashCode()));
                    return;
                }
                unit.Order.Execute();
                unit.Order.Update(GameFrame.Game.GameTime.Time);
            }
        }
    }
}