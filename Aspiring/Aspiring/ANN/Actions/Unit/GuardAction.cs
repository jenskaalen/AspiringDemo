using System;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN.Actions.Unit
{
    public class GuardAction : IUnitAction
    {
        private readonly IPopulatedArea _areaToGuard;
        private readonly IUnit _unit;

        public GuardAction(IPopulatedArea areaToGuard, IUnit unit)
        {
            _areaToGuard = areaToGuard;
            _unit = unit;
        }

        public GuardAction(IPopulatedArea areaToGuard, ISquad unit)
        {
            throw new NotImplementedException();
        }

        public IFaction Faction { get; set; }

        public double GetPriority(IFaction faction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gives executes the order for the unit
        /// </summary>
        public void Execute()
        {
            if (_unit.Order == null)
            {
                var order = new GuardAreaOrder(_areaToGuard, _unit);
                _unit.Order = order;
                order.Execute();
            }
        }
    }
}