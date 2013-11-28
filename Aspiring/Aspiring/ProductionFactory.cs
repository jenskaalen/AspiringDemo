using AspiringDemo.ANN;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;
using Ninject;
using Ninject.Modules;

namespace AspiringDemo
{
    public class ProductionFactory : NinjectModule
    {
        private static IKernel _kernel;

        public static IKernel Instance
        {
            get
            {
                if (_kernel == null)
                    _kernel = new StandardKernel(new ProductionFactory());

                return _kernel;
            }
        }

        public override void Load()
        {
            Bind<IArmy>().To<Army>();
            Bind<IFaction>().To<Faction>();
            Bind<IUnit>().To<Unit>();
            Bind<ISquad>().To<Squad>();
            Bind<ITaxes>().To<Taxes>();
            Bind<IFactionManager>().To<FactionManager>();
            Bind<IBuildingManager>().To<BuildingManager>();
            Bind<IPlacementDecider>().To<FactionPlacementDecider>();
            Bind<IRecruitmentManager>().To<RecruitmentManager>();
            Bind<IUnitManager>().To<UnitManager>();
            Bind<IZone>().To<Zone>();
            Bind<IFactionRelations>().To<FactionRelations>();
            Bind<IFactionRelation>().To<FactionRelation>();
            Bind<IItems>().To<Items>();
            Bind<ICombatModule>().To<CombatModule>();
            Bind<IUnitStats>().To<UnitStats>();
            Bind<INewFight>().To<NewFight>();
        }
    }
}