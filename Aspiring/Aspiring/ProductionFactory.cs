using System.Runtime.Serialization.Formatters;
using AspiringDemo.ANN;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Units;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Combat;

namespace AspiringDemo
{
    public class ProductionFactory : NinjectModule
    {
        public static IKernel Instance
        {
            get
            {
                if (_kernel == null)
                    _kernel = new StandardKernel(new ProductionFactory());

                return _kernel;
            }
        }

        private static IKernel _kernel;

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
            Bind<IFight>().To<Fight>();
        }
    }
}
