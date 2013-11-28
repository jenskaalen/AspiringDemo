using AspiringDemo.Sites;

namespace AspiringDemo.ANN.Actions
{
    public interface IBuildAction : IManagementAction
    {
        IPopulatedArea AreaType { get; }
        IPlacementDecider PlacementDecider { get; set; }
    }
}