using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.GameActions
{
    public class Sequence : CompositeAction
    {
        public override void Update(float elapsed)
        {
            if (Actions.Count > 0)
            {
                Actions[0].Update(elapsed);
                if (Actions[0].Finished)
                    Actions.RemoveAt(0);
            }

            Finished = Actions.Count == 0;
        }
    }
}
