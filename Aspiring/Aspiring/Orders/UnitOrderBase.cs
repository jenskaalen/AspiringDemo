using System;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Orders
{
    public class UnitOrderBase : IUnitOrder
    {
        public UnitOrderBase(IUnit unit)
        {
            Unit = unit;
        }

        protected UnitOrderBase()
        {
        }

        public IUnit Unit { get; protected set; }

        public bool IsExecuting { get; set; }

        public bool IsDone { get; set; }

        public OrderFinished Finish { get; set; }

        public string OrderName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Update(float gameTime)
        {
            throw new NotImplementedException();
        }

        protected virtual void OrderAccomplished()
        {
        }
    }
}