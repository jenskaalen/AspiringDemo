using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using AspiringDemo.Saving;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN
{
    public class BuildingManager : IBuildingManager
    {
        public AspiringDemo.Factions.IFaction Faction { get; set; }
        public List<IBuildAction> AllowedActions { get; set; }
        public IPlacementDecider PlacementDecider { get; set; }
        public List<ISerializedTypeData> BuildingSettings { get; set; }

        public BuildingManager()
        {
            AllowedActions = new List<IBuildAction>();
        }

        public BuildingManager(IFaction faction)
        {
            Faction = faction;
            AllowedActions = new List<IBuildAction>();
            //TODO: Create the outpost from settings
            var newbuild = new BuildOutpost(this.Faction);
            AllowedActions.Add(newbuild);
        }

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
            var area = buildingAction.AreaType;
            area.Zone = placementZone;

            Faction.AddArea(buildingAction.AreaType);
        }

        public Sites.IPopulatedArea CreateAreaDefaultSettings(Type type)
        {
            if (BuildingSettings == null)
                throw new Exception("Buildsettings have not been loaded");

            IPopulatedArea area = (IPopulatedArea)Activator.CreateInstance(type);
            //Type type = area.GetType();
            ISerializedTypeData typeData = BuildingSettings.FirstOrDefault(x => x.ObjectType == type);

            if (typeData == null)
                throw new Exception("Trying to access a type which is not set: " + type.Name);

            area.LoadSerializedData(typeData.SerializedData);

            return area;
        }
    }
}
