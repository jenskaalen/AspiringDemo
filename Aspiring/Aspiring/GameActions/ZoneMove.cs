using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions
{
    public class ZoneMove : GameAction
    {
        private const float ZoneTravelTime = 1.0f;
        private readonly IZone _startZone;
        private readonly IZone _targetZone;
        private readonly List<IZone> _travelPath;
        private readonly IUnit _unit;
        private float _nextZoneChange;
        private bool _started;

        //TODO: Make a zonemove that doesnt need startzone explicitly stated

        public ZoneMove(IUnit unit, IZone targetZone)
        {
            _targetZone = targetZone;
            _startZone = unit.Zone;
            _unit = unit;
            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(_startZone, _targetZone);
        }


        public ZoneMove(IUnit unit, IZone startZone, IZone targetZone)
        {
            _targetZone = targetZone;
            _startZone = startZone;
            _unit = unit;
            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(_startZone, _targetZone);
        }

        private bool ShouldWork()
        {
            return true;
        }

        public override void Update(float elapsed)
        {
            if (!ShouldWork())
                return;

            if (!_started)
            {
                if (_unit.Zone != _startZone)
                    throw new Exception("Cant start moving from a zone which the unit is not in");

                _travelPath.RemoveAt(0);

                _nextZoneChange = elapsed + ZoneTravelTime;
                _started = true;
            }

            if (_unit.Zone != _targetZone)
            {
                Finished = true;
            }

            if (elapsed > _nextZoneChange)
            {
                _nextZoneChange = elapsed + ZoneTravelTime;
                _travelPath.RemoveAt(0);
                _unit.EnterZone(_travelPath.First());
            }
        }
    }
}