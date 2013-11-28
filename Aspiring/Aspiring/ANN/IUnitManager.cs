using System.Collections.Generic;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.Factions;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN
{
    public interface IUnitManager
    {
        IFaction Faction { get; }
        List<IUnitAction> AllowedActions { get; set; }
        IUnitAction GetMostWeightedAction();
        void ExecuteAction(IManagementAction action);
        double GetAreaGuardPriority(IPopulatedArea area);
        IUnitAction AreasNeedGuarding();
        void ManageUnits();
    }
}