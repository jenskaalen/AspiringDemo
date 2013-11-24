using AspiringDemo.ANN.Actions;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.ANN
{
    public class RecruitmentManager : IRecruitmentManager
    {
        public IFaction Faction { get; set; }
        public List<IManagementAction> AllowedActions { get; set; }

        private const int UNIT_COST = 100;

        public RecruitmentManager()
        {
            AllowedActions = new List<IManagementAction>();
            AllowedActions.Add(new RecruitUnit());
        }

        public RecruitmentManager(IFaction faction)
        {
            AllowedActions = new List<IManagementAction>();
            AllowedActions.Add(new RecruitUnit());
            Faction = faction;
        }

        public IManagementAction GetMostWeightedAction(ref double priorityRequirement)
        {
            IManagementAction selectedAction = null;
            double highestVal = 0;

            foreach (IManagementAction action in AllowedActions)
            {
                if (UNIT_COST > Faction.Wealth)
                    continue;

                double val = action.GetPriority(Faction);

                if (val > highestVal && val > priorityRequirement)
                {
                    selectedAction = action;
                    priorityRequirement = val;
                }
            }

            return selectedAction;

            //throw new NotImplementedException();
        }

        //TODO: Make an interface of IRecruitUnit or smth
        public void ExecuteAction(IManagementAction action)
        {
            if (action is RecruitUnit)
            {
                Faction.CreateStandardUnit();
                //TODO: Fix magicnumber!!
                Faction.Wealth -= UNIT_COST;
            }
        }
    }
}
