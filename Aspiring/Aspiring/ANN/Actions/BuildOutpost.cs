using AspiringDemo.Factions;
using AspiringDemo.Sites;

namespace AspiringDemo.ANN.Actions
{
    public class BuildOutpost : IBuildAction
    {
        private const double FIXEDMULTIPLIER = 0.001;

        public BuildOutpost(IFaction faction)
        {
            //AreaType = typeof(Outpost);
            AreaType = new Outpost(faction, null);
            AreaType.Cost = 700;
            AreaType.AreaValue = 700;

            PlacementDecider = new FactionPlacementDecider
            {
                MaxDistanceFromCapital = int.MaxValue,
                MaxDistanceFromFactionZone = int.MaxValue,
                PreferredCapitalDistance = 2000,
                PreferredFactionZoneDistance = 2000,
                MinDistanceFromCapital = 1000,
                MinDistanceFromFactionZone = 1000
            };

            PlacementDecider.Faction = faction;
        }

        public IPopulatedArea AreaType { get; private set; }
        public IPlacementDecider PlacementDecider { get; set; }
        public IFaction Faction { get; set; }

        //public BuildOutpost() { }

        public double GetPriority(IFaction faction)
        {
            double value = faction.Wealth*faction.Power*FIXEDMULTIPLIER/faction.StructurePoints;

            return value;
        }
    }
}