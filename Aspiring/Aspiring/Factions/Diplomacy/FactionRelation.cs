using System;

namespace AspiringDemo.Factions.Diplomacy
{
    [Serializable]
    public class FactionRelation : IFactionRelation
    {
        public FactionRelation(IFaction faction)
        {
            // TODO: Complete member initialization
            Faction = faction;
            Relation = RelationType.Hostile;
        }

        public IFaction Faction { get; private set; }

        public RelationType Relation { get; set; }
    }
}