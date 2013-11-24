using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN.Actions
{
    public class RecruitUnit : IManagementAction
    {
        public IFaction Faction { get; set; }
        const double FIXEDMULTIPLIER = 1;

        public double GetPriority(IFaction faction)
        {
            double value = (faction.Wealth * FIXEDMULTIPLIER) / faction.Power;
            return value;
        }

    }
}
