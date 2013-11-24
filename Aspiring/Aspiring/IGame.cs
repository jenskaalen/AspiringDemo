using System;
using System.Collections.Generic;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using AspiringDemo.Gamecore;
using AspiringDemo.Pathfinding;
using AspiringDemo.Saving;
using Ninject;

namespace AspiringDemo
{
    public interface IGame
    {
        List<IFaction> Factions { get; set; }
        List<IWeapon> Weapons { get; set; }
        int FactionCount { get; set; }
        bool IncludeMonsters { get; set; }
        int ZonesWidth { get; set; }
        int ZonesHeight { get; set; }
        Pathing Pathfinding { get; set; }
        ISavegame Savegame { get; set; }
        IObjectFactory ObjectFactory { get; set; }
        IGameTime GameTime { get; set; }
        Pathfinder<IZone> ZonePathfinder { get; set; }
        int TimeToTravelThroughZone { get; set; }
        StandardKernel Factory { get; set; }
        void Initialize();

        [Obsolete]
        void Initialize(bool populateZones);

        void PopulateZonesAndNodes();

        /// <summary>
        /// Starts the game clock which will automatically call the GameTimeTick at set intervalls
        /// </summary>
        void StartTimer();

        Faction CreateFaction();

        /// <summary>
        /// Processes a zone and creates the necessary events - fights
        /// </summary>
        void ProcessZones();

        /// <summary>
        /// One tick of gametime
        /// </summary>
        void GametimeTick();
    }
}