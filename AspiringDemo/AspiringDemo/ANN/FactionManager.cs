using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN.Actions.Unit;
using AspiringDemo.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemo.ANN
{
    public class FactionManager : IFactionManager
    {
        public IFaction Faction { get; set; }
        public IBuildingManager BuildManager { get; set; }
        public IRecruitmentManager RecruitmentManager { get; set; }
        public IUnitManager UnitManager { get; set; }
        public IPlacementDecider PlacementDecider { get; set; }
        public int ActionsPerTurn { get; set; }
        public List<IManagementAction> QueuedActions { get; set; }

        //public FactionManager()
        //{
        //    QueuedActions = new List<IManagementAction>();
        //    //TODO: Move to constructor
        //    UnitManager = new UnitManager();
        //    UnitManager.Faction = Faction;
        //}

        public FactionManager(IFaction faction)
        {
            Faction = faction;
            QueuedActions = new List<IManagementAction>();
            RecruitmentManager = GameFrame.Game.Factory.Get<IRecruitmentManager>(new ConstructorArgument("faction", faction));
            BuildManager = GameFrame.Game.Factory.Get<IBuildingManager>(new ConstructorArgument("faction", faction));
            UnitManager = GameFrame.Game.Factory.Get<IUnitManager>(new ConstructorArgument("faction", faction));
        }

        public void ManageUnits()
        {
            if (UnitManager == null)
                return;

            double prio = 0.0;
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
            else if (buildAction != null)
            {
                QueueAction(buildAction);
                return buildAction;
            }
            else
            { 
                //nothing
                return null;
            }
        }

        private void QueueAction(IManagementAction action)
        {
            if (QueuedActions == null)
                QueuedActions = new List<IManagementAction>();

            QueuedActions.Add(action);
        }


        public void ExecuteFirstActionInQueue()
        {
            if (!QueuedActions.Any())
                return;

            var action = QueuedActions.First();

            if (action is IBuildAction)
            {
                BuildManager.ExecuteAction((IBuildAction)action);
            }
            else if (action is RecruitUnit)
            {
                RecruitmentManager.ExecuteAction(action);
            }
            else
                throw new NotImplementedException("Action type is not supported");

            QueuedActions.RemoveAt(0);
        }
    }
}
