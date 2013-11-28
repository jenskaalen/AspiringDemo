using System.Collections.Generic;

namespace AspiringDemo.Factions.Diplomacy
{
    public interface IFactionRelations
    {
        List<IFaction> Allies { get; }
        void SetRelation(IFaction faction, RelationType relation);
        IFactionRelation GetRelation(IFaction faction);
    }
}