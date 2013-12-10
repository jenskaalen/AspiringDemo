using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.Factions;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo.ANN
{
    [Serializable]
    public class FactionManager : IFactionManager
    {
        public FactionManager(IFaction faction)
        {
            Faction = faction;
            QueuedActions = new List<IManagementAction>();
            RecruitmentManager =
                GameFrame.Game.Factory.Get<IRecruitmentManager>(new ConstructorArgument("faction", faction));
            BuildManager = GameFrame.Game.Factory.Get<IBuildingManager>(new ConstructorArgument("faction", faction));
            UnitManager = GameFrame.Game.Factory.Get<IUnitManager>(new ConstructorArgument("faction", faction));
        }

        public IFaction Faction { get; set; }
        public IBuildingManager BuildManager { get; set; }
        public IRecruitmentManager RecruitmentManager { get; set; }
        public IUnitManager UnitManager { get; set; }
        public IPlacementDecider PlacementDecider { get; set; }
        public int ActionsPerTurn { get; set; }
        public List<IManagementAction> QueuedActions { get; set; }

        public void ManageUnits()
        {
            if (UnitManager == null)
                return;

            IUnitAction action = UnitManager.GetMostWeightedAction();

            if (action is GuardAction)
            {
                action.Execute();
            }

            UnitManager.ManageUnits();
        }

        public IManagementAction DetermineAction()
        {
            double highestActionPriority = 0;

            //TODO: try to make this in a smoother way
            IBuildAction buildAction = BuildManager.GetMostWeightedAction(ref highestActionPriority);
            IManagementAction recruitAction = RecruitmentManager.GetMostWeightedAction(ref highestActionPriority);

            if (recruitAction != null)
            {
                QueueAction(recruitAction);
                return recruitAction;
            }
            if (buildAction != null)
            {
                QueueAction(buildAction);
                return buildAction;
            }
            //nothing
            return null;
        }


        public void ExecuteFirstActionInQueue()
        {
            if (!QueuedActions.Any())
                return;

            IManagementAction action = QueuedActions.First();

            if (action is IBuildAction)
            {
                BuildManager.ExecuteAction((IBuildAction) action);
            }
            else if (action is RecruitUnit)
            {
                RecruitmentManager.ExecuteAction(action);
            }
            else
                throw new NotImplementedException("Action type is not supported");

            QueuedActions.RemoveAt(0);
        }

        private void QueueAction(IManagementAction action)
        {
            if (QueuedActions == null)
                QueuedActions = new List<IManagementAction>();

            QueuedActions.Add(action);
        }
    }
}