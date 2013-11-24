using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Orders
{
    public delegate void OrderFinished();
    public delegate int OrderTick();

    public interface IUnitOrder
    {
        IUnit Unit { get; }
        bool IsExecuting { get; set; }
        bool IsDone { get; set; }
        void Execute();
        void Work(long gameTime);
        OrderFinished Finish { get; set; }
        string OrderName { get;  }
    }
}
