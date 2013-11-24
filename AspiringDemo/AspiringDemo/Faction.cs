using AspiringDemo.ANN;
using AspiringDemo.Orders;
using AspiringDemo.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Faction : AspiringDemo.IFaction
    {
        public int ID { get; set; }
        public List<Squad> Squads { get; set; }
        public bool IsComputer { get; set; }
        public string Name { get; set; }
        public IFactionManager FactionManager { get; set; }
        public int Wealth { get; set; }
        public int Power { get; set; }
        public int StructurePoints { get; set; }
        public List<Sites.IPopulatedArea> Areas { get; set; }
        public Zone Capital { get; set; }
        public List<Unit> Units { get; set; }
        //public List<Zone> GameZones
        //{
        //    //TODO: Fizz this
        //    //get;
        //    //set;
        //    private set { throw new NotImplementedException(); }
        //    get
        //    {
        //        if (_game != null)
        //            return _game.ZonePathfinder.Nodes;
        //        else
        //            return null;
        //    }
        //}

        public IObjectFactory ObjectFactory { get; set; }

        private IObjectFactory _factory;
        private Game _game = null;

        public Faction()
        {
            Squads = new List<Squad>();
        }

        public Faction(IObjectFactory factory)
        {
            _factory = factory;
            Squads = new List<Squad>();
        }

        public void Initialize(IObjectFactory factory, Game game)
        {
            _factory = factory;
            _game = game;
            FactionManager = new FactionManager();
            FactionManager.BuildManager = new BuildingManager(this);
            FactionManager.UnitManager = new UnitManager(this);
            Areas = new List<Sites.IPopulatedArea>();
            Units = new List<Unit>();
            Squads = new List<Squad>();
        }

        public Squad CreateSquad()
        {
            //TODO: rework defaults.. ?
            Squad newSquad = _factory.GetObject<Squad>();
            newSquad.Faction = this;
            Squads.Add(newSquad);

            return newSquad;
        }

        public Unit CreateUnit()
        {
            //TODO: Rework defaults
            Unit newUnit = _factory.GetObject<Unit>();
            newUnit.Weapons = new List<IWeapon>();
            newUnit.Weapons.Add(new Unarmed());
            newUnit.Faction = this;

            Units.Add(newUnit);

            return newUnit;
        }

        public void RegisterOrder(IUnitOrder order, Unit unit)
        {
            unit.Order = order;
            order.Unit = unit;
            _game.GameTime.TimeTicker += order.Work;
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

        public List<Zone> GetGameZones()
        {
            return _game.ZonePathfinder.Nodes;
        }


        public void AddArea(Sites.IPopulatedArea area)
        {
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
    }
}
