using AspiringDemo.ANN;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Orders;
using AspiringDemo.Roleplaying;
using AspiringDemo.Sites;
using AspiringDemo.Units;
using AspiringDemo.Weapons;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspiringDemo.GameCore;

namespace AspiringDemo.Factions
{
    public delegate IUnit UnitCreationDelegate();

    public class Faction : IFaction
    {
        public int ID { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public IFactionManager FactionManager { get; set; }
        public UnitCreationDelegate CreateUnit { get; set; }
        public int Wealth { get; set; }
        public int Power
        {
            get 
            {
                return Army.Units.Count * 100;
            }
            set
            {
                // remove set implementation
                _power = value;
            }
        }

        public int StructurePoints
        {
            get 
            {
                if (Areas.Count == 0)
                    return 0;

                int structurePoints = Areas.Select(x => x.AreaValue).Aggregate ((a, b) => a + b);
                return structurePoints;
            }
            set 
            {
                //TODO: Remove
            }
        }

        public List<Sites.IPopulatedArea> Areas
        {
            get;
            set;
        }
            //return GameFrame.Game.ZonePathfinder.Nodes.SelectMany(zone => zone.PopulatedAreas).Where(area => area.Owner == this).ToList();

            
        public IZone CapitalZone { get; set; }
        public ITaxes Taxes { get; set; }

        public IArmy Army { get; set; }
        public IObjectFactory ObjectFactory { get; set; }

        private int _power;
        //TODO: Remove
        private Random _random = new Random();
        private bool _initialized = false;

        public StrengthMeasurement Strength { get; set; }
        public IFactionRelations Relations { get; set; }

        public Faction()
        {
            Areas = new List<IPopulatedArea>();
            Army = GameFrame.Game.Factory.Get<IArmy>();
            FactionManager = GameFrame.Game.Factory.Get<FactionManager>(new ConstructorArgument("faction", this));
            FactionManager.BuildManager = GameFrame.Game.Factory.Get<IBuildingManager>(new ConstructorArgument("faction", this));
            FactionManager.RecruitmentManager = GameFrame.Game.Factory.Get<IRecruitmentManager>(new ConstructorArgument("faction", this));
            FactionManager.UnitManager = GameFrame.Game.Factory.Get<IUnitManager>(new ConstructorArgument("faction", this));
            Relations = GameFrame.Game.Factory.Get<IFactionRelations>(new ConstructorArgument("faction", this));
            Areas = new List<Sites.IPopulatedArea>();
            CreateUnit += CreateStandardUnit;
        }

        //public Faction(IObjectFactory factory)
        //{
        //    Areas = new List<IPopulatedArea>();
        //    Army = ProductionFactory.Instance.Get<IArmy>();
        //    FactionManager = ProductionFactory.Instance.Get<FactionManager>(new ConstructorArgument("faction", this));
        //    FactionManager.BuildManager = ProductionFactory.Instance.Get<IBuildingManager>(new ConstructorArgument("faction", this));
        //    FactionManager.RecruitmentManager = ProductionFactory.Instance.Get<IRecruitmentManager>(new ConstructorArgument("faction", this));
        //    FactionManager.UnitManager = ProductionFactory.Instance.Get<IUnitManager>(new ConstructorArgument("faction", this));
        //    Areas = new List<Sites.IPopulatedArea>();
        //    CreateUnit += CreateStandardUnit;
        //}

        public void Initialize()
        {
            // possibly throw an exception here just to weed out bad code (i.e. calling initializer several times)
            if (_initialized)
                return;

            _initialized = true;

            if (GameFrame.Game.GameTime != null)
                GameFrame.Game.GameTime.TimeTicker += GameTimeTick;
        }

        public ISquad CreateSquad()
        {
            //TODO: rework defaults.. ?
            Squad newSquad = GameFrame.Game.Factory.Get<Squad>();
            //newSquad.Faction = this;
            //Squads.Add(newSquad);
            Army.Squads.Add(newSquad);

            return newSquad;
        }

        public IUnit CreateStandardUnit()
        {
            //TODO: Rework defaults
            IUnit newUnit = GameFrame.Game.Factory.Get<Unit>(new ConstructorArgument("faction", this));
            newUnit.Weapons = new List<IWeapon>();
            newUnit.Weapons.Add(new Unarmed());
            Army.Units.Add(newUnit);

            //TODO: Only spawnable in eeas is now
            if (CapitalZone != null)
                CapitalZone.EnterZone(newUnit);

            return newUnit;
        }

        public void UnitChangedState(UnitState state)
        {
            if (state == UnitState.Dead)
            { 
                
            }
        }

        public T GetObject<T>() where T : class, new()
        {
            T obj = new T();

            return obj;
        }

        public List<IZone> GetGameZones()
        {
            return GameFrame.Game.ZonePathfinder.Nodes;
        }


        public void AddArea(Sites.IPopulatedArea area)
        {
            if (Areas == null)
                Areas = new List<Sites.IPopulatedArea>();

            Areas.Add(area);
            Wealth -= area.Cost;
            StructurePoints += area.AreaValue;
        }

        public void RemoveArea(Sites.IPopulatedArea area)
        {
            Areas.Remove(area); 
            Wealth += area.Cost;
            StructurePoints -= area.AreaValue;
        }

        public void GameTimeTick(long time)
        {

            //regen hp
            //TODO: "Cache" alive units
            foreach (var unit in Army.Units.Where(unit => unit.State == UnitState.Idle || unit.State == UnitState.ExecutingOrder ))
                unit.TimeTick(time);

            // tax-collecting
            if (Taxes != null)
            {
                Wealth += Taxes.CollectTaxes(Areas, Taxes.TaxPerPayer, GameFrame.Game.GameTime.Time);
            }

            // get and execute action
            FactionManager.DetermineAction();
            FactionManager.ExecuteFirstActionInQueue();

            //new order module - work in progress
            FactionManager.ManageUnits();

            // assign orders to idle units without a squad
            //List<IUnit> idleSquadlessUnits = Army.GetIdleUnits();

            //TODO: Just sending them to a random spot, scrap and rework this
            //foreach (IUnit unit in idleSquadlessUnits)
            //{
            //    var randomzone = GetRandomZone();

            //    if (randomzone == unit.Zone)
            //        continue;

            //    var path = GameFrame.Game.ZonePathfinder.GetPath(unit.Zone, randomzone);

            //    if (path.Count < 2)
            //        continue;
                
            //    //order.TargetZone = GetRandomZone();
            //    TravelOrder order = new TravelOrder(path, unit);
            //    RegisterOrder(order, unit);
            //}

            //// assign orders to idle squads
            //var idleSquads = Army.Squads.Where(x => x.State == SquadState.Idle);
            ////TODO: Just sending them to a random spot, rework this
            //foreach (Squad squad in idleSquads)
            //{
            //    var randomzone = GetRandomZone();

            //    if (randomzone == squad.Leader.Zone)
            //        continue;

            //    var path = GameFrame.Game.ZonePathfinder.GetPath(squad.Leader.Zone, randomzone);

            //    TravelOrder travelorder = new TravelOrder(path, squad);
            //}

            // work orders
            foreach (var unit in Army.Units.Where(unit => unit.Order != null))
            {
                //TODO: Should the order be removed?
                if (unit.Order.IsDone)
                    continue;

                if (!unit.Order.IsExecuting)
                    unit.Order.Execute();

                if (unit.State == UnitState.Idle)
                    unit.State = UnitState.ExecutingOrder;

                unit.Order.Work(time);
            }
        }

        //TODO: used for testing - remove when proper AI is enabled
        private IZone GetRandomZone()
        {
            var nodes = GetGameZones();
            //int randomIndex = new Random().Next(0, nodes.Count - 1);
            int randomIndex = _random.Next(0, nodes.Count - 1);

            return nodes[randomIndex];
        }

        //TODO: used for testing - remove when proper AI is enabled
        private IZone GetRandomZone(int seed)
        {
            var nodes = GetGameZones();
            int randomIndex = new Random().Next(0, nodes.Count - 1);

            return nodes[randomIndex];
        }
    }
}
