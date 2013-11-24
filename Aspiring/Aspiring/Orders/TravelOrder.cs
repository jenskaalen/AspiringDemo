using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;
using Ninject.Planning.Targets;

namespace AspiringDemo.Orders
{
    public class TravelOrder : UnitOrderBase
    {
        //public IUnit Unit { get; set; }
        //public bool IsExecuting { get; set; }
        //public bool IsDone { get; set; }
        //public OrderFinished Finish { get; set; }
        public IZone TargetZone { get; set; }
        public List<IZone> TravelPath { get; set; }
        public new string OrderName { get { return this.ToString();  } }

        private long _nextWorkTime;
        private bool _waitOnComplete = false;

        public TravelOrder(IEnumerable<IZone> travelpath, IUnit unit, bool waitOnComplete)
        {

            if (travelpath.Count() == 0)
            {
                Unit = unit;
                TargetZone = unit.Zone;

                GameFrame.Debug.Log("Travelpath is empty for Travelorder given to " + unit.GetHashCode());
                return;
            }

            // this trick copies the items of the list
            TravelPath = travelpath.ToList();
            TargetZone = TravelPath.Last();
            _nextWorkTime = GameFrame.Game.TimeToTravelThroughZone;
            Unit = unit;

            _waitOnComplete = waitOnComplete;
        }

        public TravelOrder(IUnit unit, IZone targetZone)
        {
            if (unit.Zone == null)
                throw new NullReferenceException("Unit.Zone can't be null when issuing travelorder");

            TravelPath = GameFrame.Game.ZonePathfinder.GetPath(unit.Zone, targetZone);
            TargetZone = targetZone;
            Unit = unit;
        }

        public static void GiveTravelOrder(ISquad squad, IZone targetZone, bool waitOnComplete)
        {
            var lastTravelPath = new List<IZone>();

            foreach (var member in squad.Members)
            {
                TravelOrder unitOrder;

                if (member.Zone == targetZone)
                {
                    unitOrder = new TravelOrder(new List<IZone>() { targetZone}, member, waitOnComplete );
                    member.AssignOrder(unitOrder);
                    continue;
                }


                if (lastTravelPath.Any() && member.Zone == lastTravelPath.First())
                {
                    // create new order
                    unitOrder = new TravelOrder(lastTravelPath, member, waitOnComplete);
                    member.AssignOrder(unitOrder);
                    // order is executed by the faction manager...
                    continue;
                }

                lastTravelPath = GameFrame.Game.ZonePathfinder.GetPath(member.Zone, targetZone);
                unitOrder = new TravelOrder(lastTravelPath, member, waitOnComplete);
                member.AssignOrder(unitOrder);
            }
        }


        public static void GiveTravelOrder(IUnit unit, IZone targetZone, bool waitOnComplete)
        {
            var lastTravelPath = new List<IZone>();

            TravelOrder unitOrder;

            if (unit.Zone == targetZone)
            {
                unitOrder = new TravelOrder(new List<IZone>() { targetZone }, unit, waitOnComplete);
                unit.AssignOrder(unitOrder);
            }
            else if (lastTravelPath.Any() && unit.Zone == lastTravelPath.First())
            {
                // create new order
                unitOrder = new TravelOrder(lastTravelPath, unit, waitOnComplete);
                unit.AssignOrder(unitOrder);
            }

            lastTravelPath = GameFrame.Game.ZonePathfinder.GetPath(unit.Zone, targetZone);
            unitOrder = new TravelOrder(lastTravelPath, unit, waitOnComplete);
            unit.AssignOrder(unitOrder);
        }

        public override void Execute()
        {
            if (Unit == null)
                throw new Exception("Cant execute without unit set");

            if (TargetZone == null)
                throw new Exception("TargetZone cannot be null");

            Unit.State = UnitState.ExecutingOrder;
            IsExecuting = true;
            IsDone = false;
        }

        public override void Work(long gameTime)
        {
            if (!IsExecuting)
                return;

            if (Unit.Zone == TargetZone)
            {
                OrderAccomplished();
                return;
            }

            if (Unit.State == UnitState.ExecutingOrder)
            {
                if (Unit.Zone.IsPlayerNearby)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    if (_nextWorkTime < gameTime)
                    {
                        var enteredZone = TravelPath.First();
                        enteredZone.EnterZone(Unit);
                        TravelPath.Remove(Unit.Zone);
                        _nextWorkTime += GameFrame.Game.TimeToTravelThroughZone;

                        //TODO: Uhh, remove?
                        if (TravelPath.Count == 1 && TravelPath[0] != TargetZone)
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        protected override void OrderAccomplished()
        {
            Unit.Order = null;
            IsExecuting = false;
            IsDone = true;

            if (Unit.State == UnitState.ExecutingOrder)
            {
                Unit.State = _waitOnComplete ? UnitState.Waiting : UnitState.Idle;
            }
        }
    }
}
