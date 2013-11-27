using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using AspiringDemo.Units;
using Ninject;

namespace AspiringDemo.Factions
{
    public interface IFaction : AspiringDemo.GameCore.IObjectFactory
    {
        UnitCreationDelegate CreateUnit { get; set; }
        System.Collections.Generic.List<AspiringDemo.Sites.IPopulatedArea> Areas { get; set; }
        ISquad CreateSquad();
        IUnit CreateStandardUnit();
        AspiringDemo.ANN.IFactionManager FactionManager { get; set; }
        void Initialize();
        IZone CapitalZone { get; set; }
        ITaxes Taxes { get; set; }
        bool IsComputer { get; set; }
        string Name { get; set; }
        int Power { get; set; }
        int Wealth { get; set; }
        int StructurePoints { get; set; }
        int ID { get; set; }
        List<IZone> GetGameZones();
        void AddArea(IPopulatedArea area);
        void RemoveArea(IPopulatedArea area);
        void GameTimeTick(float time);
        IArmy Army { get; set; }
        StrengthMeasurement Strength { get; set; }
        IFactionRelations Relations { get; set; }
    }
}
