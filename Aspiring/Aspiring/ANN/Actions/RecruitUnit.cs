using AspiringDemo.Factions;

namespace AspiringDemo.ANN.Actions
{
    public class RecruitUnit : IManagementAction
    {
        private const double FIXEDMULTIPLIER = 1;
        public IFaction Faction { get; set; }

        public double GetPriority(IFaction faction)
        {
            double value = (faction.Wealth*FIXEDMULTIPLIER)/faction.Power;
            return value;
        }
    }
}