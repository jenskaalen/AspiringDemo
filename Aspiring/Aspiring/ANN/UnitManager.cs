using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.ANN.War;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN
{
    public class UnitManager : IUnitManager
    {
        private const double AreaGuardModifier = 1.0;
        private const int AreaGuardsRequirement = 3;
        private readonly List<AttackAction> _currentAttackActions;
        private readonly IWarmodule _warmodule;

        public UnitManager(IFaction faction)
        {
            if (faction == null)
                throw new ArgumentException("Faction can't be null");

            Faction = faction;
            _currentAttackActions = new List<AttackAction>();
            _warmodule = new Warmodule();
        }

        public IFaction Faction { get; private set; }
        public List<IUnitAction> AllowedActions { get; set; }

        public void ManageUnits()
        {
            SquadFormAction.FormSquad(Faction);

            List<AttackAction> attacksToRemove = _currentAttackActions.Where(attack => attack.AttackStarted).ToList();
            attacksToRemove.ForEach(atck => _currentAttackActions.Remove(atck));

            if (Faction.Strength > StrengthMeasurement.Abysmal && _currentAttackActions.Count == 0)
            {
                _currentAttackActions.Add(new AttackAction(Faction,
                    _warmodule.BestAreaToAttackFromAllFactions(Faction).Zone, Faction.CapitalZone, 5));
            }

            _currentAttackActions.ForEach(action => action.Work());
        }

        public IUnitAction GetMostWeightedAction()
        {
            IUnitAction action = AreasNeedGuarding();
            return action;
        }

        public IUnitAction AreasNeedGuarding()
        {
            double highestPrio = 0;
            IUnitAction action = null;

            foreach (IPopulatedArea area in Faction.Areas)
            {
                //double priority = (double)Faction.Army.AliveUnits.Count(unit => unit.Order.GetType() == typeof(GuardAreaOrder)) / Faction.Areas.Count;

                double areaPrio = GetAreaGuardPriority(area);

                if (areaPrio > 0.99 && areaPrio > highestPrio)
                {
                    highestPrio = areaPrio;
                    IPopulatedArea guardArea = area;
                    IUnit idleUnit = Faction.Army.Units.FirstOrDefault(unit => unit.State == UnitState.Idle);

                    if (idleUnit != null)
                    {
                        action = new GuardAction(guardArea, idleUnit);
                    }
                }
            }

            return action;
        }

        public double GetAreaGuardPriority(IPopulatedArea area)
        {
            int guardingUnits =
                Faction.Army.AliveUnits.Count(
                    unit => unit.Order is GuardAreaOrder && ((GuardAreaOrder) unit.Order).TargetArea == area);

            double areaPrio = AreaGuardsRequirement - guardingUnits;

            return areaPrio;
        }

        public void ExecuteAction(IManagementAction action)
        {
            throw new NotImplementedException();
        }
    }
}