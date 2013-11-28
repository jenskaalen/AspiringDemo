using AspiringDemo.Factions;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN.War
{
    public interface IWarmodule
    {
        IFaction BestFactionToAttack(IFaction selfFaction);
        IPopulatedArea BestAreaToAttack(IFaction targetFaction);
        IPopulatedArea BestAreaToAttackFromAllFactions(IFaction selfFaction);
    }
}