using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using AspiringDemo.Saving;
using AspiringDemo.Sites;
using AspiringDemo.Zones;

namespace AspiringDemo.ANN
{
    public class BuildingManager : IBuildingManager
    {
        public BuildingManager()
        {
            AllowedActions = new List<IBuildAction>();
        }

        public BuildingManager(IFaction faction)
        {
            Faction = faction;
            AllowedActions = new List<IBuildAction>();
            var newbuild = new BuildOutpost(Faction);
            AllowedActions.Add(newbuild);
        }

        public IPlacementDecider PlacementDecider { get; set; }

        public IFaction Faction { get; set; }
        public List<IBuildAction> AllowedActions { get; set; }
        public List<ISerializedTypeData> BuildingSettings { get; set; }

        public IBuildAction GetMostWeightedAction(ref double priorityRequirement)
        {
            IBuildAction selectedAction = null;
            double highestVal = 0;

            foreach (IBuildAction action in AllowedActions)
            {
                if (action.AreaType.Cost > Faction.Wealth)
                    continue;

                double val = action.GetPriority(Faction);

                if (val > highestVal && val > priorityRequirement)
                {
                    selectedAction = action;
                    priorityRequirement = val;
                }
            }

            return selectedAction;
        }

        public void ExecuteAction(IBuildAction buildingAction)
        {
            IZone placementZone = buildingAction.PlacementDecider.GetBestZone(Faction.GetGameZones());

            //TODO: get default area settings somehow
            //IPopulatedArea area = CreateAreaDefaultSettings(buildingAction.A reaType);
            //TODO: one area of entry!
            placementZone.AddArea(buildingAction.AreaType);
            IPopulatedArea area = buildingAction.AreaType;
            area.Zone = placementZone;

            Faction.AddArea(buildingAction.AreaType);
        }

        public IPopulatedArea CreateAreaDefaultSettings(Type type)
        {
            if (BuildingSettings == null)
                throw new Exception("Buildsettings have not been loaded");

            var area = (IPopulatedArea) Activator.CreateInstance(type);
            //Type type = area.GetType();
            ISerializedTypeData typeData = BuildingSettings.FirstOrDefault(x => x.ObjectType == type);

            if (typeData == null)
                throw new Exception("Trying to access a type which is not set: " + type.Name);

            area.LoadSerializedData(typeData.SerializedData);

            return area;
        }
    }
}