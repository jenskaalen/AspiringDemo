namespace AspiringDemo.Factions.Diplomacy
{
    public interface IFactionRelation
    {
        IFaction Faction { get; }
        RelationType Relation { get; set; }
    }
}