using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions.Movement
{
    [Serializable]
    public class MoveFromExteriorToInterior : Sequence
    {
        private IUnit _unit;
        private IZone _interiorZone;

        public MoveFromExteriorToInterior(IUnit unit, IZone interiorZone)
        {
            _unit = unit;
            _interiorZone = interiorZone;
            var exteriorZones = interiorZone.ZoneEntrances.Select(entrance => entrance.Zone);
            IZone closestExteriorZone = exteriorZones.OrderBy(zone => Utility.GetDistance(unit.Zone.Position, zone.Position)).FirstOrDefault();

            Add(new ZoneMove(unit, unit.Zone, closestExteriorZone));
            Add(new EnterInterior(unit, interiorZone));
        }
    }
}
