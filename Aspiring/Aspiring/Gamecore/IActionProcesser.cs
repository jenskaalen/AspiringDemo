using System.Collections.Generic;
using AspiringDemo.GameActions;

namespace AspiringDemo.Gamecore
{
    public interface IActionProcesser
    {
        List<GameAction> Actions { get; set; }
        void Update(float time);
    }
}
