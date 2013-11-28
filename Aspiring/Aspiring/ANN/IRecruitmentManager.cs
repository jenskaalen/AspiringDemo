using System.Collections.Generic;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN
{
    public interface IRecruitmentManager
    {
        IFaction Faction { get; set; }
        List<IManagementAction> AllowedActions { get; set; }
        IManagementAction GetMostWeightedAction(ref double priority);

        void ExecuteAction(IManagementAction action);
    }
}