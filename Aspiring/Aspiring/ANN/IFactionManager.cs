using System.Collections.Generic;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN
{
    public interface IFactionManager
    {
        IFaction Faction { get; set; }
        IBuildingManager BuildManager { get; set; }
        IRecruitmentManager RecruitmentManager { get; set; }
        IPlacementDecider PlacementDecider { get; set; }
        IUnitManager UnitManager { get; set; }
        List<IManagementAction> QueuedActions { get; set; }
        //List<IAction
        int ActionsPerTurn { get; set; }
        IManagementAction DetermineAction();
        void ManageUnits();
        void ExecuteFirstActionInQueue();
    }
}