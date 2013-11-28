using System;
using System.Collections.Generic;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using AspiringDemo.Saving;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN
{
    public interface IBuildingManager
    {
        IFaction Faction { get; set; }
        List<IBuildAction> AllowedActions { get; set; }
        List<ISerializedTypeData> BuildingSettings { get; set; }
        IBuildAction GetMostWeightedAction(ref double priority);

        IPopulatedArea CreateAreaDefaultSettings(Type type);
        void ExecuteAction(IBuildAction buildingAction);
    }
}