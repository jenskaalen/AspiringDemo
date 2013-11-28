﻿namespace AspiringDemo.GameActions
{
    public class Parallel : CompositeAction
    {
        public override void Update(float elapsed)
        {
            Actions.ForEach(a => a.Update(elapsed));
            Actions.RemoveAll(a => a.Finished);
            Finished = Actions.Count == 0;
        }
    }
}