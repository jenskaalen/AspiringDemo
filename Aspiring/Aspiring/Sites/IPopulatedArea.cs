using AspiringDemo.Factions;
using AspiringDemo.Saving;
using AspiringDemo.Zones;

namespace AspiringDemo.Sites
{
    public interface IPopulatedArea : ICustomSerializable
    {
        IFaction Owner { get; set; }
        bool Razed { get; set; }
        bool IsUnderAttack { get; set; }
        int AreaValue { get; set; }
        int BuildTime { get; set; }
        int Cost { get; set; }
        int Population { get; set; }
        IZone Zone { get; set; }
    }
}