using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public enum ActionType
    {
        Attack,
        Buff,
        Debuff
    }

    public class Action
    {
        public int HPModifier { get; set; }
        public int SpeedModifier { get; set; }
    }
}
