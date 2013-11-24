using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Orders
{
    //No need for it. Can be deleted
    public class FollowOrder : IUnitOrder
    {
        public IUnit FollowTarget { get; set; }
        public bool IsPlayerNearby { get; set; }
        public IUnit Unit
        {
            get;
            set;
        }
        public string OrderName { get { return this.ToString(); } }

        public bool IsExecuting
        {
            get;
            set;
        }

        public bool IsDone
        {
            get;
            set;
        }

        public OrderFinished Finish
        {
            get;
            set;
        }

        public FollowOrder(IUnit follower, IUnit followTarget)
        {
            FollowTarget = followTarget;
            Unit = follower;
        }

        //TODO: Order never gets finished
        public void Work(long gameTime)
        {
            if (!IsPlayerNearby)
            {
                if (FollowTarget != Unit.Zone)
                {
                    FollowTarget.Zone.EnterZone(Unit);
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
