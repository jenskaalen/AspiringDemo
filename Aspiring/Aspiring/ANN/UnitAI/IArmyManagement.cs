using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN.UnitAI
{
    interface IArmyManagement
    {
        IFaction Faction { get; set; }
        List<IManagementAction> AllowedActions { get; set; }
        IManagementAction GetMostWeightedAction(ref double priority);
    }
}
