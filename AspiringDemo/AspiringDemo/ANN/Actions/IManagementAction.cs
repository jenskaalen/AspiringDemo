using AspiringDemo.Factions;

namespace AspiringDemo.ANN.Actions
{
    public interface IManagementAction
    {
        IFaction Faction { get; set; }
        double GetPriority(IFaction faction);
    }
}
