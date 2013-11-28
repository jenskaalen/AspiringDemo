using System;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;

namespace AspiringDemo.ANN
{
    public class ActionManager
    {
        public IFaction Faction { get; set; }

        public IManagementAction GetMostWeightedAction()
        {
            throw new NotImplementedException();
        }
    }
}