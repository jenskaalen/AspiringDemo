using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using AspiringDemo.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN
{
    public interface IBuildingManager
    {
        IFaction Faction { get; set; }
        List<IBuildAction> AllowedActions { get; set; }
        IBuildAction GetMostWeightedAction(ref double priority);
        List<ISerializedTypeData> BuildingSettings { get; set; }

        Sites.IPopulatedArea CreateAreaDefaultSettings(Type type);
        void ExecuteAction(IBuildAction buildingAction);
    }
}
