using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN.War
{
    public class Warmodule : IWarmodule
    {
        public Factions.IFaction BestFactionToAttack(Factions.IFaction selfFaction)
        {
            var possibleTargets = GameFrame.Game.Factions.Where(faction => faction != selfFaction && faction.Areas.Any()).ToArray();

            if (possibleTargets.Length == 0)
                throw new Exception("There is only one faction in the game");

            return possibleTargets[GameFrame.Random.Next(0, possibleTargets.Length)];
        }

        public Sites.IPopulatedArea BestAreaToAttack(Factions.IFaction targetFaction)
        {
            return targetFaction.Areas[GameFrame.Random.Next(0, targetFaction.Areas.Count)];
        }


        public Sites.IPopulatedArea BestAreaToAttackFromAllFactions(IFaction selfFaction)
        {
            IFaction bestFaction = BestFactionToAttack(selfFaction);
            var bestArea = bestFaction.Areas[GameFrame.Random.Next(0, bestFaction.Areas.Count)];
            return bestArea;
        }
    }
}
