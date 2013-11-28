using AspiringDemo;
using AspiringDemo.ANN;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringImplementation
{
    public class StandardFactory : NinjectModule
    {
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
            Bind<IFactionRelation>().To<IFactionRelation>();
        }
    }
}
