using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameCore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Sites;
using AspiringDemo.Weapons;
using AspiringDemo.Zones;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo.Factions
{
    [Serializable]
    public delegate IUnit UnitCreationDelegate();

    [Serializable]
    public class Faction : IFaction
    {
        private readonly Random _random = new Random();
        private bool _initialized;

        public Faction()
        {
            Areas = new List<IPopulatedArea>();
            Army = GameFrame.Game.Factory.Get<IArmy>();
            FactionManager = GameFrame.Game.Factory.Get<FactionManager>(new ConstructorArgument("faction", this));
            FactionManager.BuildManager =
                GameFrame.Game.Factory.Get<IBuildingManager>(new ConstructorArgument("faction", this));
            FactionManager.RecruitmentManager =
                GameFrame.Game.Factory.Get<IRecruitmentManager>(new ConstructorArgument("faction", this));
            FactionManager.UnitManager =
                GameFrame.Game.Factory.Get<IUnitManager>(new ConstructorArgument("faction", this));
            Relations = GameFrame.Game.Factory.Get<IFactionRelations>(new ConstructorArgument("faction", this));
            Areas = new List<IPopulatedArea>();
            CreateUnit += CreateStandardUnit;
        }

        public IObjectFactory ObjectFactory { get; set; }

        public int ID { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public IFactionManager FactionManager { get; set; }
        public UnitCreationDelegate CreateUnit { get; set; }
        public int Wealth { get; set; }

        public int Power
        {
            get { return Army.Units.Count*100; }
        }

        public int StructurePoints
        {
            get
            {
                if (Areas.Count == 0)
                    return 0;

                int structurePoints = Areas.Select(x => x.AreaValue).Aggregate((a, b) => a + b);
                return structurePoints;
            }
            set
            {
                //TODO: Remove
            }
        }

        public List<IPopulatedArea> Areas { get; set; }
        public IZone CapitalZone { get; set; }
        public ITaxes Taxes { get; set; }
        public IArmy Army { get; set; }
        public StrengthMeasurement Strength { get; set; }
        public IFactionRelations Relations { get; set; }

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
            var newSquad = GameFrame.Game.Factory.Get<Squad>();
            //newSquad.Faction = this;
            //Squads.Add(newSquad);
            Army.Squads.Add(newSquad);

            return newSquad;
        }

        public IUnit CreateStandardUnit()
        {
            //TODO: Rework defaults
            IUnit newUnit = GameFrame.Game.Factory.Get<Unit>(new ConstructorArgument("faction", this));
            newUnit.Items.Weapons = new List<IWeapon>();
            newUnit.Items.Weapons.Add(new Unarmed());
            Army.Units.Add(newUnit);

            //TODO: Only spawnable in eeas is now
            if (CapitalZone != null)
                newUnit.EnterZone(CapitalZone);

            return newUnit;
        }

        public T Create<T>() where T : IUnit
        {
            //TODO: Rework defaults
            T newUnit = GameFrame.Game.Factory.Get<T>(new ConstructorArgument("faction", this));
            newUnit.Items.Weapons = new List<IWeapon>();
            newUnit.Items.Weapons.Add(new Unarmed());
            Army.Units.Add(newUnit);

            //TODO: Only spawnable in eeas is now
            if (CapitalZone != null)
                newUnit.EnterZone(CapitalZone);

            return newUnit;
        }

        public List<IZone> GetGameZones()
        {
            return GameFrame.Game.ZonePathfinder.Nodes;
        }


        public void AddArea(IPopulatedArea area)
        {
            if (Areas == null)
                Areas = new List<IPopulatedArea>();

            Areas.Add(area);
            Wealth -= area.Cost;
            StructurePoints += area.AreaValue;
        }

        public void RemoveArea(IPopulatedArea area)
        {
            Areas.Remove(area);
            Wealth += area.Cost;
            StructurePoints -= area.AreaValue;
        }

        public void GameTimeTick(float time)
        {
            //regen hp
            //TODO: "Cache" alive units
            foreach (
                IUnit unit in
                    Army.Units.Where(unit => unit.State == UnitState.Idle || unit.State == UnitState.ExecutingOrder))
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
            foreach (IUnit unit in Army.Units.Where(unit => unit.Order != null))
            {
                //TODO: Should the order be removed?
                if (unit.Order.IsDone)
                    continue;

                if (!unit.Order.IsExecuting)
                    unit.Order.Execute();

                if (unit.State == UnitState.Idle)
                    unit.State = UnitState.ExecutingOrder;

                unit.Order.Update(time);
            }
        }

        public void UnitChangedState(UnitState state)
        {
            if (state == UnitState.Dead)
            {
            }
        }

        //TODO: used for testing - remove when proper AI is enabled
        private IZone GetRandomZone()
        {
            List<IZone> nodes = GetGameZones();
            //int randomIndex = new Random().Next(0, nodes.Count - 1);
            int randomIndex = _random.Next(0, nodes.Count - 1);

            return nodes[randomIndex];
        }

        //TODO: used for testing - remove when proper AI is enabled
        private IZone GetRandomZone(int seed)
        {
            List<IZone> nodes = GetGameZones();
            int randomIndex = new Random().Next(0, nodes.Count - 1);

            return nodes[randomIndex];
        }


        public IFactionUnits FactionUnits { get; set; }

    }
}