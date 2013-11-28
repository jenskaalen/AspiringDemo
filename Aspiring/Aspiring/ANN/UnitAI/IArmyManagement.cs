using System.Collections.Generic;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN.UnitAI
{
    internal interface IArmyManagement
    {
        IFaction Faction { get; set; }
        List<IManagementAction> AllowedActions { get; set; }
        IManagementAction GetMostWeightedAction(ref double priority);
    }
}