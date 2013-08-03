using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    interface IDecisionMaker
    {
        Character Character { get; set; }

        Ability NextDecision();
    }
}
