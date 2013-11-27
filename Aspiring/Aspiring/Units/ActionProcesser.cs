using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameActions;

namespace AspiringDemo.Units
{
    public class ActionProcesser : IActionProcesser
    {
        public List<GameAction> Actions { get; set; }
        public void Update(float time)
        {
            throw new NotImplementedException();
        }
    }
}
