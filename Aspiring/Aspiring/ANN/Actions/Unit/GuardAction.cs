using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Orders;
using AspiringDemo.Sites;
using AspiringDemo.Units;

namespace AspiringDemo.ANN.Actions.Unit
{
    public class GuardAction : IUnitAction
    {
        public Factions.IFaction Faction { get; set; }
        private readonly IUnit _unit;
        private readonly IPopulatedArea _areaToGuard;

        public GuardAction(IPopulatedArea areaToGuard, IUnit unit)
        {
            _areaToGuard = areaToGuard;
            _unit = unit;
        }

        public GuardAction(IPopulatedArea areaToGuard, ISquad unit)
        {
            throw new NotImplementedException();
        }

        public double GetPriority(Factions.IFaction faction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gives executes the order for the unit
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
