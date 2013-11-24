using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
