using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Orders
{
    public interface ICharacterOrder
    {
        Character Character { get; set; }
        bool IsExecuting { get; set; }
        bool IsDone { get; set; }
        void Execute();
        void Work();
    }
}
