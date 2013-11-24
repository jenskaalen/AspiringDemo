using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.ANN.War;
using AspiringDemo.Orders;
using AspiringDemo.Sites;
using AspiringDemo.Factions;
using AspiringDemo.Units;

namespace AspiringDemo.ANN
{
    public class UnitManager : IUnitManager
    {
        public IFaction Faction { get; private set; }
        public List<Actions.Unit.IUnitAction> AllowedActions { get; set; }

        private List<AttackAction> _currentAttackActions; 
        private const double AreaGuardModifier = 1.0;
        private const int AreaGuardsRequirement = 3;
        private IWarmodule _warmodule;

        public UnitManager(IFaction faction)
        {
            if (faction == null)
                throw new ArgumentException("Faction can't be null");

            Faction = faction;
            _currentAttackActions = new List<AttackAction>();
            _warmodule = new Warmodule();
        }

        public void ManageUnits()
        {
            SquadFormAction.FormSquad(Faction);

            var attacksToRemove = _currentAttackActions.Where(attack => attack.AttackStarted).ToList();
            attacksToRemove.ForEach(atck => _currentAttackActions.Remove(atck));

            if (Faction.Strength > StrengthMeasurement.Abysmal && _currentAttackActions.Count == 0)
            {
                _currentAttackActions.Add(new AttackAction(Faction, _warmodule.BestAreaToAttackFromAllFactions(this.Faction).Zone, this.Faction.CapitalZone, 5));
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

            foreach (var area in Faction.Areas)
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

        public void ExecuteAction(Actions.IManagementAction action)
        {
            throw new NotImplementedException();
        }
    }
}
