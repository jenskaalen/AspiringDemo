using System;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions
{
    public class Patrol : Sequence
    {
        public Patrol(IUnit unit, IZone zoneA, IZone zoneB, int patrols)
        {
            // first move to the target zone
            Actions.Add(new ZoneMove(unit, zoneA));

            for (int i = 0; i < patrols; i++)
            {
                // move to location
                Actions.Add(new ZoneMove(unit, zoneB));
                // and back again
                Actions.Add(new ZoneMove(unit, zoneA));
            }
        }

        public override void Update(float elapsed)
        {
            throw new NotImplementedException();
        }
    }
}