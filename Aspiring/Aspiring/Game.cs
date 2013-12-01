using System;
using System.Collections.Generic;
using System.Threading;
using AspiringDemo.Factions;
using AspiringDemo.GameCore;
using AspiringDemo.Gamecore;
using AspiringDemo.Pathfinding;
using AspiringDemo.Saving;
using AspiringDemo.Zones;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo
{
    //TODO: Extract interface and replace concretions
    // or?
    public class Game : IGame
    {
        private StandardKernel _kernel = new StandardKernel(new ProductionFactory());
        private StrengthMap _strengthMap;
        private bool _timerStarted = false;

        public Game()
        {
            GameTime = new GameTime();
            ActionProcesser = new ActionProcesser();
        }

        public Game(ISavegame savegame, IObjectFactory factory)
        {
            GameTime = new GameTime();
            Savegame = savegame;
            ObjectFactory = factory;
            ActionProcesser = new ActionProcesser();
        }

        private int MilisecondsPerTimeTick { get; set; }

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
        public IActionProcesser ActionProcesser { get; set; }

        //const int ZoneWidth = 500;
        //const int ZoneHeight = 500;

        //private int _milisecondsPerTimeTick = 1000;

        public StandardKernel Factory
        {
            get { return _kernel; }
            set { _kernel = value; }
        }

        public void Initialize()
        {
            GameTime.SecondsPerTick = MilisecondsPerTimeTick;
            Factions = new List<IFaction>();
            PopulateZonesAndNodes();

            if (GameTime.SecondsPerTick == 0)
            {
                GameTime.SecondsPerTick = 1f;
            }
        }


        [Obsolete]
        public void Initialize(bool populateZones)
        {
            GameTime.SecondsPerTick = MilisecondsPerTimeTick;
            Factions = new List<IFaction>();

            if (GameTime.SecondsPerTick == 0)
            {
                GameTime.SecondsPerTick = 1f;
            }

            if (populateZones)
                PopulateZonesAndNodes();
        }

        public void PopulateZonesAndNodes()
        {
            //TODO: Dont use magic numbers for zones
            Pathfinding = new Pathing();
            Pathfinding.Zones = (List<IZone>) CreateZones(499, 499);
            ZonePathfinder = new Pathfinder<IZone>();
            ZonePathfinder.Nodes = CreateZones(499, 499);
        }

        /// <summary>
        ///     Starts the game clock which will automatically call the GameTimeTick at set intervalls
        /// </summary>
        public void StartTimer()
        {
            if (!_timerStarted)
            {
                var timerThread = new Thread(GameTickTimeLoop);
                timerThread.Start();
            }
        }

        public Faction CreateFaction()
        {
            if (Factions == null)
                Factions = new List<IFaction>();

            var newFaction = _kernel.Get<Faction>(new ConstructorArgument("factory", ObjectFactory));
            newFaction.Initialize();
            Factions.Add(newFaction);

            if (GameTime != null)
                _strengthMap = new StrengthMap(GameTime.Time);

            return newFaction;
        }

        /// <summary>
        ///     Processes a zone and creates the necessary events - fights
        /// </summary>
        public void ProcessZones()
        {
            foreach (Zone zone in Pathfinding.Zones)
            {
                //TODO: remove this - zones no longer control the fights
                // zone.Fight.PerformFightRound();
            }
        }

        /// <summary>
        ///     One tick of gametime
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

                ActionProcesser.Update(GameTime.Time);
            }
        }

        private List<IZone> CreateZones(int width, int height)
        {
            var zones = new List<IZone>();

            for (int i = 0; i < ZonesWidth; i++)
            {
                for (int j = 0; j < ZonesHeight; j++)
                {
                    IZone newZone = new Zone
                        (
                            width*i + 1,
                            width * i + width,
                            height,
                            width
                        );
                    //IZone newZone = new Zone();
                    //newZone.Area.X1 = width*i + 1;
                    //newZone.X1 = width*i + width;
                    //newZone.PositionYStart = height*j + 1;
                    //newZone.PositionYEnd = height*j + height;

                    zones.Add(newZone);
                }
            }

            return zones;
        }

        /// <summary>
        ///     Keeps ticking time in a loop
        /// </summary>
        private void GameTickTimeLoop()
        {
            while (true)
            {
                GametimeTick();
                Thread.Sleep((int) (GameTime.SecondsPerTick*1000));
            }
        }
    }
}