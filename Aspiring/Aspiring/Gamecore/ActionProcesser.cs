using System;
using System.Collections.Generic;
using AspiringDemo.GameActions;

namespace AspiringDemo.Gamecore
{
    [Serializable]
    public class ActionProcesser : IActionProcesser
    {
        public ActionProcesser()
        {
            Actions = new List<GameAction>();
        }

        public List<GameAction> Actions { get; set; }

        public void Update(float time)
        {
            Actions.ForEach(a => a.Update(time));
            Actions.RemoveAll(a => a.Finished);
        }
    }
}