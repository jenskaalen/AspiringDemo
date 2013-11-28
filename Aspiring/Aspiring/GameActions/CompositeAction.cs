using System.Collections.Generic;

namespace AspiringDemo.GameActions
{
    public abstract class CompositeAction : GameAction
    {
        public List<GameAction> Actions { get; protected set; }

        public void Add(GameAction action)
        {
            Actions.Add(action);
        }
    }
}