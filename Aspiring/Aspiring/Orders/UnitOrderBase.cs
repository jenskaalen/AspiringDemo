using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Orders
{
    public class UnitOrderBase : IUnitOrder
    {
        public IUnit Unit { get; protected set; }

        public bool IsExecuting { get; set; }

        public bool IsDone { get; set; }

        public OrderFinished Finish { get; set; }

        public string OrderName
        {
            get { throw new NotImplementedException(); }
        }

        public UnitOrderBase(IUnit unit)
        {
            Unit = unit;
        }

        protected UnitOrderBase()
        {
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Work(long gameTime)
        {
            throw new NotImplementedException();
        }

        protected virtual void OrderAccomplished()
        {
        }
    }
}
