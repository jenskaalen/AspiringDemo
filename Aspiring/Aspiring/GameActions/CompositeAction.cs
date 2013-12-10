using System;
using System.Collections.Generic;

namespace AspiringDemo.GameActions
{
    [Serializable]
    public abstract class CompositeAction : GameAction
    {
        public List<GameAction> Actions { get; protected set; }

        public void Add(GameAction action)
        {
            if (Actions == null)
                Actions = new List<GameAction>();

            Actions.Add(action);
        }
    }
}