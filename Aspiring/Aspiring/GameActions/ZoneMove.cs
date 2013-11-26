using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore;
using AspiringDemo.Units;

namespace AspiringDemo.GameActions
{
    public class ZoneMove : GameAction
    {
        private readonly IZone _targetTargetZone;
        private readonly IZone _startZone;
        private List<IZone> _travelPath;
        private float _nextZoneChange;
        private bool _started = false;

        private const float ZoneTravelTime = 1.0f;

        //TODO: Make a zonemove that doesnt need startzone explicitly stated

        public ZoneMove(IUnit unit, IZone startZone, IZone targetZone)
        {
            _targetTargetZone = targetZone;
            _startZone = unit.Zone;

            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(_startZone, _targetTargetZone);

            // travel path..
        }

        public override void Update(float elapsed)
        {
            if (!_started)
            {
                _nextZoneChange = elapsed + ZoneTravelTime;
                _started = true;
            }

            if (elapsed > _nextZoneChange)
            {
                _nextZoneChange = elapsed + ZoneTravelTime;

            }
        }
    }
}
