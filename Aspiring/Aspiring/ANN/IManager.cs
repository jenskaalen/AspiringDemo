using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN
{
    public interface IManager
    {
        IFaction Faction { get; set; }
        List<IManagementAction> AllowedActions { get; set; }
        IManagementAction GetMostWeightedAction(ref double priority);

        void ExecuteAction(IManagementAction action);
    }
}
