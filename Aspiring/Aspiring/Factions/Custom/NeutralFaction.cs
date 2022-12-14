using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Sites;
using AspiringDemo.Zones;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo.Factions.Custom
{
    [Serializable]
    public class NeutralFaction : INeutralFaction
    {
        public NeutralFaction()
        {
            Areas = new List<IPopulatedArea>();
            Army = GameFrame.Game.Factory.Get<IArmy>();
            //FactionManager = GameFrame.Game.Factory.Get<FactionManager>(new ConstructorArgument("faction", this));
            //FactionManager.BuildManager = GameFrame.Game.Factory.Get<IBuildingManager>(new ConstructorArgument("faction", this));
            //FactionManager.RecruitmentManager = GameFrame.Game.Factory.Get<IRecruitmentManager>(new ConstructorArgument("faction", this));
            //FactionManager.UnitManager = GameFrame.Game.Factory.Get<IUnitManager>(new ConstructorArgument("faction", this));
            Relations = GameFrame.Game.Factory.Get<IFactionRelations>(new ConstructorArgument("faction", this));
            Areas = new List<IPopulatedArea>();
            CreateUnit += CreateStandardUnit;
        }

        private bool _initialized { get; set; }

        public UnitCreationDelegate CreateUnit { get; set; }
        public IFactionRelations Relations { get; set; }
        public List<IPopulatedArea> Areas { get; set; }

        public ISquad CreateSquad()
        {
            //TODO: rework defaults.. ?
            var newSquad = GameFrame.Game.Factory.Get<ISquad>();
            Army.Squads.Add(newSquad);

            return newSquad;
        }

        public IUnit CreateStandardUnit()
        {
            var zed = new Zombie(this);
            IZone zone =
                GameFrame.Game.ZonePathfinder.Nodes[GameFrame.Random.Next(0, GameFrame.Game.ZonePathfinder.Nodes.Count)];
            Army.AddUnit(zed);

            zed.EnterZone(zone);

            return zed;
        }

        public IFactionManager FactionManager { get; set; }

        public void Initialize()
        {
            // possibly throw an exception here just to weed out bad code (i.e. calling initializer several times)
            if (_initialized)
                return;

            _initialized = true;

            if (GameFrame.Game.GameTime != null)
                GameFrame.Game.GameTime.TimeTicker += GameTimeTick;
        }

        public IZone CapitalZone { get; set; }

        public ITaxes Taxes { get; set; }

        public bool IsComputer { get; set; }

        public string Name { get; set; }

        public int Power { get; set; }

        public int Wealth { get; set; }

        public int StructurePoints { get; set; }

        public int ID { get; set; }

        public List<IZone> GetGameZones()
        {
            throw new NotImplementedException();
        }

        public void AddArea(IPopulatedArea area)
        {
            throw new NotImplementedException();
        }

        public void RemoveArea(IPopulatedArea area)
        {
            throw new NotImplementedException();
        }

        public void GameTimeTick(float time)
        {
            bool isZombieTime = GameFrame.Random.Next(0, 30) == 1;

            SquadFormAction.FormSquad(this, 8);

            if (isZombieTime && Army.Squads.Count == 0)
            {
                // create zombie squad
                IZone targetZone =
                    GameFrame.Game.ZonePathfinder.Nodes.Where(zone => !zone.PopulatedAreas.Any())
                        .ToList()
                        .GetRandomNode();

                int zombieAmount = GameFrame.Random.Next(7, 18);

                ISquad squad = CreateSquad();

                for (int i = 0; i < zombieAmount; i++)
                    squad.AddMember(CreateStandardUnit(targetZone));
            }

            foreach (ISquad squad in Army.Squads)
            {
                // order squad around
                if (squad.State == SquadState.Idle)
                    TravelOrder.GiveTravelOrder(squad, Utility.GetRandomZone(), false);
            }

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

        public IArmy Army { get; set; }

        public StrengthMeasurement Strength { get; set; }

        public T GetObject<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IUnit CreateStandardUnit(IZone zone)
        {
            var zed = new Zombie(this);
            zed.EnterZone(zone);
            Army.AddUnit(zed);

            return zed;
        }


        public IFactionUnits FactionUnits { get; set; }


        public T Create<T>() where T : IUnit
        {
            throw new NotImplementedException();
        }
    }
}