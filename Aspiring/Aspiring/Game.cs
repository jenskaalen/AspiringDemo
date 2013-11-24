using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspiringDemo.Pathfinding;
using System.Threading;
using System.Diagnostics;
using AspiringDemo.Gamecore;
using AspiringDemo.Saving;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo
{
    //TODO: Extract interface and replace concretions
    // or?
    public class Game : IGame
    {
        public List<IFaction> Factions { get; set; }
        public List<IWeapon> Weapons { get; set; }
        public int FactionCount { get; set; }
        public bool IncludeMonsters { get; set; }
        public int ZonesWidth { get; set; }
        public int ZonesHeight { get; set; }
        public Pathing Pathfinding { get; set; }
        public ISavegame Savegame { get; set; }
        public IObjectFactory ObjectFactory { get; set; }
        public IGameTime GameTime { get; set; }
        public Pathfinder<IZone> ZonePathfinder { get; set; }
        //public const int TimeToTravelThroughZone = 1;
        public int TimeToTravelThroughZone { get; set; }

        //const int ZoneWidth = 500;
        //const int ZoneHeight = 500;

        //private int _milisecondsPerTimeTick = 1000;
        private int MilisecondsPerTimeTick { get; set; }

        private bool _timerStarted = false;
        private StrengthMap _strengthMap;
        public StandardKernel Factory 
        {
            get { return _kernel; }
            set { _kernel = value;  }
        }

        private StandardKernel _kernel = new StandardKernel(new ProductionFactory());

        public Game()
        {
            GameTime = new GameTime();
        }

        public Game(ISavegame savegame, IObjectFactory factory)
        {
            Savegame = savegame;
            ObjectFactory = factory;
        }

        public void Initialize()
        {
            GameTime = new GameTime();
            GameTime.MilisecondsPerTick = MilisecondsPerTimeTick;
            Factions = new List<IFaction>();
            PopulateZonesAndNodes();

            if (GameTime.MilisecondsPerTick == 0)
            {
                GameTime.MilisecondsPerTick = 1000;
            }
        }

        [Obsolete]
        public void Initialize(bool populateZones)
        {
            GameTime.MilisecondsPerTick = MilisecondsPerTimeTick;
            Factions = new List<IFaction>();

            if (GameTime.MilisecondsPerTick == 0)
            {
                GameTime.MilisecondsPerTick = 1000;
            }

            if (populateZones)
                PopulateZonesAndNodes();
        }

        public void PopulateZonesAndNodes()
        {
            //TODO: Dont use magic numbers for zones
            Pathfinding = new Pathing();
            Pathfinding.Zones = CreateZones(500, 500);
            ZonePathfinder = new Pathfinder<IZone>();
            ZonePathfinder.Nodes = CreateZones(500, 500);
        }

        /// <summary>
        /// Starts the game clock which will automatically call the GameTimeTick at set intervalls
        /// </summary>
        public void StartTimer()
        {
            if (!_timerStarted)
            {
                Thread timerThread = new Thread(GameTickTimeLoop);
                timerThread.Start();
            }
        }

        private List<IZone> CreateZones(int width, int height)
        {
            List<IZone> zones = new List<IZone>();

            for (int i = 0; i < ZonesWidth; i++)
            {
                for (int j = 0; j < ZonesHeight; j++)
                {
                    IZone newZone = new Zone();
                    newZone.PositionXStart = width * i + 1;
                    newZone.PositionXEnd = width * i + width;
                    newZone.PositionYStart = height * j + 1;
                    newZone.PositionYEnd = height * j + height;
                    zones.Add(newZone);
                }
            }

            return zones;
        }

        public Faction CreateFaction()
        {
            if (Factions == null)
                Factions = new List<IFaction>();

            Faction newFaction = _kernel.Get<Faction>(new ConstructorArgument("factory", this.ObjectFactory));
            newFaction.Initialize();
            Factions.Add(newFaction);

            if (GameTime != null)
                _strengthMap = new StrengthMap(GameTime.Time);

            return newFaction;
        }

        /// <summary>
        /// Processes a zone and creates the necessary events - fights
        /// </summary>
        public void ProcessZones()
        {
            foreach (Zone zone in Pathfinding.Zones)
            {
                zone.Fight.PerformFightRound();
            }
        }

        /// <summary>
        /// Keeps ticking time in a loop
        /// </summary>
        private void GameTickTimeLoop()
        {
            while (true)
            {
                GametimeTick();
                System.Threading.Thread.Sleep(GameTime.MilisecondsPerTick);
            }
        }

        /// <summary>
        /// One tick of gametime
        /// </summary>
        public void GametimeTick()
        {
            if (!GameTime.GamePaused)
            {
                GameTime.Time++;

                if (GameTime.TimeTicker != null)
                {
                    GameTime.TimeTicker(GameTime.Time);
                }

                foreach (var zone in ZonePathfinder.Nodes.Where(x => x.Fight != null))
                    zone.Fight.PerformFightRound();
            }
        }


    }
}
