using System;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Orders
{
    //No need for it. Can be deleted
    public class FollowOrder : IUnitOrder
    {
        public FollowOrder(IUnit follower, IUnit followTarget)
        {
            FollowTarget = followTarget;
            Unit = follower;
        }

        public IUnit FollowTarget { get; set; }
        public bool IsPlayerNearby { get; set; }
        public IUnit Unit { get; set; }

        public string OrderName
        {
            get { return ToString(); }
        }

        public bool IsExecuting { get; set; }

        public bool IsDone { get; set; }

        public OrderFinished Finish { get; set; }

        //TODO: Order never gets finished
        public void Update(float gameTime)
        {
            if (!IsPlayerNearby)
            {
                if (FollowTarget != Unit.Zone)
                {
                    Unit.EnterZone(FollowTarget.Zone);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public void Execute()
        {
            //throw new NotImplementedException();
            Unit.State = UnitState.ExecutingOrder;
        }
    }
}