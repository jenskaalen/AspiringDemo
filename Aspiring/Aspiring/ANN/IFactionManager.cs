using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
