using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.GameActions
{
    public abstract class CompositeAction : GameAction
    {
        public void Add(GameAction action)
        {
            Actions.Add(action);
        }

        public List<GameAction> Actions { get; protected set; }
    }
}
