using System;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN.War
{
    public class Warmodule : IWarmodule
    {
        public IFaction BestFactionToAttack(IFaction selfFaction)
        {
            IFaction[] possibleTargets =
                GameFrame.Game.Factions.Where(faction => faction != selfFaction && faction.Areas.Any()).ToArray();

            if (possibleTargets.Length == 0)
                throw new Exception("There is only one faction in the game");

            return possibleTargets[GameFrame.Random.Next(0, possibleTargets.Length)];
        }

        public IPopulatedArea BestAreaToAttack(IFaction targetFaction)
        {
            return targetFaction.Areas[GameFrame.Random.Next(0, targetFaction.Areas.Count)];
        }


        public IPopulatedArea BestAreaToAttackFromAllFactions(IFaction selfFaction)
        {
            IFaction bestFaction = BestFactionToAttack(selfFaction);
            IPopulatedArea bestArea = bestFaction.Areas[GameFrame.Random.Next(0, bestFaction.Areas.Count)];
            return bestArea;
        }
    }
}