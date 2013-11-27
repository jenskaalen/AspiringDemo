using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameActions;

namespace AspiringDemo.Units
{
    public interface IActionProcesser
    {
        List<GameAction> Actions { get; set; }
        void Update(float time);
    }
}
