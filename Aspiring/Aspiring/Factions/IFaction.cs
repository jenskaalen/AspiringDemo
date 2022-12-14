using System;
using System.Collections.Generic;
using AspiringDemo.ANN;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.GameCore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Sites;
using AspiringDemo.Zones;

namespace AspiringDemo.Factions
{
    public interface IFaction
    {
        //UnitCreationDelegate CreateUnit { get; set; }
        List<IPopulatedArea> Areas { get; set; }
        IFactionUnits FactionUnits { get; set; }
        IFactionManager FactionManager { get; set; }
        IZone CapitalZone { get; set; }
        ITaxes Taxes { get; set; }
        bool IsComputer { get; set; }
        string Name { get; set; }
        int Power { get; }
        int Wealth { get; set; }
        int StructurePoints { get; set; }
        int ID { get; set; }
        IArmy Army { get; set; }
        StrengthMeasurement Strength { get; set; }
        IFactionRelations Relations { get; set; }
        ISquad CreateSquad();
        void Initialize();
        List<IZone> GetGameZones();
        void AddArea(IPopulatedArea area);
        void RemoveArea(IPopulatedArea area);
        void GameTimeTick(float time);
        T Create<T>() where T : IUnit;
    }
}