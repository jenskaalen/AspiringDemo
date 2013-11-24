using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using AspiringDemo.ANN.Actions.Unit;
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
