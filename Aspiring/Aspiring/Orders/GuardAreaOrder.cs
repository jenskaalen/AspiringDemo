using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Sites;

namespace AspiringDemo.Orders
{
    public class GuardAreaOrder : IUnitOrder
    {
        private readonly IZone _targetZone;
        private readonly List<IZone> _travelPath;
        private long _nextWorkTime;
        private IZone _startZone;

        public GuardAreaOrder(IPopulatedArea targetArea, IUnit unit)
        {
            TargetArea = targetArea;
            _nextWorkTime = GameFrame.Game.TimeToTravelThroughZone;
            Unit = unit;

            //TODO: optimize
            IZone first = null;
            foreach (IZone zone in GameFrame.Game.ZonePathfinder.Nodes)
            {
                if (zone.PopulatedAreas.Contains(targetArea))
                {
                    first = zone;
                    break;
                }
            }
            _targetZone =
                first;

            // construct travelpath
            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(unit.Zone, _targetZone);
        }

        public IPopulatedArea TargetArea { get; set; }
        public IUnit Unit { get; set; }
        public bool IsExecuting { get; set; }
        public bool IsDone { get; set; }

        public string OrderName
        {
            get { return ToString(); }
        }

        public OrderFinished Finish { get; set; }


        public void Execute()
        {
            if (Unit == null)
                throw new Exception("Cant execute without unit set");

            if (_targetZone == null)
                throw new Exception("TargetZone cannot be null");

            Unit.State = UnitState.ExecutingOrder;
            _startZone = Unit.Zone;
            IsExecuting = true;
            IsDone = false;
        }

        public void Update(float gameTime)
        {
            if (!IsExecuting)
                return;

            if (Unit.Zone == _targetZone)
            {
                //OrderAccomplished();
                // yay - we are in the right zone, we stay here
                return;
            }

            if (Unit.State == UnitState.ExecutingOrder)
            {
                if (Unit.Zone.IsPlayerNearby)
                {
                    throw new NotImplementedException();
                }
                if (_nextWorkTime < gameTime)
                {
                    IZone enteredZone = _travelPath.First();
                    Unit.EnterZone(enteredZone);
                    _travelPath.Remove(Unit.Zone);
                    _nextWorkTime += GameFrame.Game.TimeToTravelThroughZone;

                    //TODO: Uhh, remove?
                    if (_travelPath.Count == 1 && _travelPath[0] != _targetZone)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }
    }
}