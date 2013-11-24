using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Orders
{
    public class AttackLocationOrder : ICharacterOrder
    {
        public Unit Unit
        {
            get;
            set;
        }

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

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Work(long gameTime)
        {
            throw new NotImplementedException();
        }

        public OrderFinished Finish
        {
            get;
            set;
        }
    }
}
