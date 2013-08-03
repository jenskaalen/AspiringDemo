using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspiringDemo.Pathfinding;
using System.Threading;
using System.Diagnostics;

namespace AspiringDemo
{
    public static class Game
    {
        public static List<Faction> Factions { get; set; }
        public static List<Weapon> Weapons { get; set; }
        public static int FactionCount { get; set; }
        public static bool IncludeMonsters { get; set; }
        public static int ZonesWidth { get; set; }
        public static int ZonesHeight { get; set; }
        public static Pathing Pathfinding { get; set; }
        public static ISavegame SaveGame { get; set; }
        public static IObjectFactory ObjectFactory { get; set; }
        public static long GameTime { get; set; }
        public static bool GamePaused { get; set; }


        public static Pathfinder<Zone> ZonePathfinder { get; set; }
        //public static IObjectGenerator ObjectGenerator { get; set; }
        //TODO: Rework
        public const int TimeToTravelThroughZone = 15;

        const int zoneWidth = 500;
        const int zoneHeight = 500;

        public static void Initialize()
        {
            Factions = new List<Faction>();
            Pathfinding = new Pathing();
            Pathfinding.Zones = CreateZones();
            ZonePathfinder = new Pathfinder<Zone>();
            ZonePathfinder.Nodes = CreateZones();
            Thread timerThread = new Thread(CountGametime);
            timerThread.Start();
            Thread orderThread = new Thread(WorkOrders);
            orderThread.Start();
        }

        private static List<Zone> CreateZones()
        {
            List<Zone> zones = new List<Zone>();

            for (int i = 0; i < ZonesWidth; i++)
            {
                for (int j = 0; j < ZonesHeight; j++)
                {
                    Zone newZone = new Zone();
                    newZone.PositionXStart = zoneWidth * i + 1;
                    newZone.PositionXEnd = zoneWidth * i + zoneWidth;
                    newZone.PositionYStart = zoneHeight * j + 1;
                    newZone.PositionYEnd = zoneHeight * j + zoneHeight;
                    zones.Add(newZone);
                }
            }

            return zones;
        }

        public static void AddFaction(Faction faction)
        {
            if (Factions == null)
                Factions = new List<Faction>();

            Factions.Add(faction);
        }

        public static bool IsZoneContested(Zone zone)
        {
            //TODO: Doesnt take account for relations
            int factionsInZone = 0;

            foreach (Faction faction in Game.Factions)
            {
                if (faction.Squads.Where(x => x.State != SquadState.Destroyed && x.Zone == zone).Any())
                    factionsInZone++;
            }

            if (factionsInZone > 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>Returns null if no zone is found</returns>
        public static List<Squad> IsZoneContestedSquads(Zone zone)
        {
            List<Squad> squads = new List<Squad>();
            int factionsInZone = 0;

            foreach (Faction faction in Game.Factions)
            {
                if (faction.Squads.Where(x => x.State != SquadState.Destroyed && x.Zone == zone).Any())
                {
                    factionsInZone++;
                    squads.AddRange(faction.Squads.Where(x => x.Zone == zone));
                }
            }

            if (factionsInZone > 1)
                return squads;
            else
                return null;
        }

        /// <summary>
        /// Processes a zone and creates the necessary events - fights
        /// </summary>
        public static void ProcessZones()
        {
            foreach (Zone zone in Pathfinding.Zones)
            {
                ProcessZone(zone);
            }
        }

        public static void ProcessZone(Zone zone)
        {
            if (zone.Fight != null)
            {
                if (zone.Fight.FightActive)
                {
                    zone.Fight.PerformFightRound();
                }
                else
                {
                    //fight has ended so we clean up
                    zone.Fight = null;
                }
            }
            else
            {
                List<Squad> contestedZoneSquads = IsZoneContestedSquads(zone);

                if (contestedZoneSquads != null)
                {
                    foreach (var squad in contestedZoneSquads)
                    {
                        squad.State = SquadState.Fighting;
                    }

                    if (zone.Fight == null)
                    {
                        Fight zoneFight = new Fight();
                        //TODO: rework this to use custom adding method?
                        zoneFight.Squads.AddRange(contestedZoneSquads);
                        zone.Fight = zoneFight;
                    }
                }
            }
        }

        public static void CountGametime()
        {
            while (true)
            {
                if (!GamePaused)
                    GameTime++;

                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void WorkOrders()
        {
            while (true)
            {
                Debug.WriteLine("working orders");

                foreach (Faction faction in Factions)
                {
                    foreach (Squad squad in faction.Squads)
                    {
                        foreach (Character character in squad.Members)
                        {
                            if (character.Order != null)
                            {
                                if (character.Order.IsExecuting)
                                {
                                    character.Order.Work();
                                }
                                else
                                {
                                    if (character.Order.IsDone)
                                    {
                                        character.Order = null;
                                    }
                                    else
                                    {
                                        character.Order.Execute();
                                    }
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
