using AspiringDemo;
using AspiringDemo.Factions.Custom;
using AspiringDemo.Saving;
using AspiringDemo.Zones;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameCore;
using AspiringDemo.Factions;
using AspiringDemo.ANN;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Sites;

namespace AspiringImplementation
{
    public class GameRig : IGameRig
    {
        public StandardFactory Factory { get; set; }
        public int Worldsize { get; set; }
        public int FactionCount { get; set; }

        public void RigGame()
        {
            try
            {
                TestSave savegame = new TestSave("saveu.sdf");
                GameFrame.Game.Savegame = savegame;
                GameFrame.Game.ObjectFactory = savegame;
                GameFrame.Game.ZonesHeight = Worldsize;
                GameFrame.Game.ZonesWidth = Worldsize;
                GameFrame.Game.Initialize();
                GameFrame.Game.ZonePathfinder.Nodes = GetZones(Worldsize, Worldsize);

                for (int i = 0; i < FactionCount; i++)
                {
                    GameFrame.Game.CreateFaction();
                }

                Random random = new Random();

                foreach (IFaction faction in GameFrame.Game.Factions)
                {
                    faction.Wealth = 2000;
                    //TODO: Fix - this can def. backfire it duplicate capitals happen
                    //TODO: Random here might not be quite right
                    faction.CapitalZone = GameFrame.Game.ZonePathfinder.Nodes[GameFrame.Random.Next(0, GameFrame.Game.ZonePathfinder.Nodes.Count - 1)];
                    // rig factions with some gold income
                    PopulatedArea area = new PopulatedArea(faction, faction.CapitalZone);
                    area.Population = 100;
                    ITaxes taxes = new Taxes();
                    taxes.CollectionRate = 10;
                    taxes.TaxPerPayer = 1;

                    faction.Taxes = taxes;

                    faction.Initialize();

                    //TODO: LOOK AT THIS LOGIC!?! Fix it!
                    faction.AddArea(area);
                    faction.CapitalZone.AddArea(area);
                }

                // rig zombiefaction
                IFaction zfac = new NeutralFaction();
                zfac.Name = "Zombies";

                //for (int i=0; i < 30; i++)
                //    zfac.Create<Unit>();

                GameFrame.Game.Factions.Add(zfac);
                zfac.Initialize();
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        private IFaction GetFaction() 
        {
            IFaction faction = Factory.Kernel.Get<IFaction>();
            faction.FactionManager = GetFactionManager(faction);

            return faction;
        }

        /// <summary>
        /// Gets faction manager together with all the required underlying objects
        /// </summary>
        /// <returns></returns>
        private IFactionManager GetFactionManager(IFaction faction)
        {
            IFactionManager manager = Factory.Kernel.Get<IFactionManager>();
            manager.BuildManager = GetBuildingManager(faction);
            manager.RecruitmentManager = GetUnitManager();

            return manager;
        }

        private IBuildingManager GetBuildingManager(IFaction faction) 
        {
            IBuildingManager manager = Factory.Kernel.Get<IBuildingManager>();
            manager.AllowedActions = GetBuildingAllowedActions(faction);

            return manager;
        }

        private List<AspiringDemo.ANN.Actions.IBuildAction> GetBuildingAllowedActions(IFaction faction)
        {
            List<IBuildAction> actions = new List<IBuildAction>();
            BuildOutpost bo = new BuildOutpost(faction);

            return actions;
        }

        private IRecruitmentManager GetUnitManager() 
        {
            IRecruitmentManager manager = Factory.Kernel.Get<IRecruitmentManager>();
            manager.AllowedActions = GetUnitManagerAllowedActions();

            return manager;
        }

        private List<IManagementAction> GetUnitManagerAllowedActions()
        {
            List<IManagementAction> actions = new List<IManagementAction>();
            RecruitUnit recruitUnit = new RecruitUnit();
            actions.Add(recruitUnit);

            return actions;
        }

        /// <summary>
        /// General placement decider...
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        private IPlacementDecider GetPlacementDecider()
        {
            IPlacementDecider decider = Factory.Kernel.Get<IPlacementDecider>();

            return decider;
        }


        public static List<IZone> GetZones(int height, int width)
        {
            List<IZone> zones = new List<IZone>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    IZone zone = new Zone(i*1000, j*1000, 999, 999);

                    zones.Add(zone);
                }
            }

            foreach (var zone in zones)
            {
                var nbors = Neighbours(zones, zone, 1000);

                zone.Neighbours = new List<IZone>();
                zone.Neighbours = nbors;
            }

            return zones;
        }

        public static List<IZone> Neighbours(List<IZone> zones, IZone checkzone, int size)
        {
            var neighbours = new List<IZone>();

            int xPos = checkzone.Area.X1;
            int yPos = checkzone.Area.Y1;

            var zone1 = zones.FirstOrDefault(x => x.Area.X1 == (xPos - 1000) && x.Area.Y1 == yPos);
            var zone2 = zones.FirstOrDefault(x => x.Area.X1 == (xPos + 1000) && x.Area.Y1 == yPos);
            var zone3 = zones.FirstOrDefault(x => x.Area.Y1 == (yPos - 1000) && x.Area.X1 == xPos);
            var zone4 = zones.FirstOrDefault(x => x.Area.Y1 == (yPos + 1000) && x.Area.X1 == xPos);

            if (zone1 != null)
                neighbours.Add(zone1);

            if (zone2 != null)
                neighbours.Add(zone2);

            if (zone3 != null)
                neighbours.Add(zone3);

            if (zone4 != null)
                neighbours.Add(zone4);

            return neighbours;
        }
    }
}
