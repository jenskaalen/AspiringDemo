using System;
using System.Collections.Generic;
using System.Linq;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;
using Ninject.Planning.Targets;

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
            if (targetZone == unit.Zone)
            {
                Finished = true;
                return;
            }

            _targetZone = targetZone;
            _startZone = unit.Zone;
            _unit = unit;
            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(_startZone, _targetZone);
        }


        public ZoneMove(IUnit unit, IZone startZone, IZone targetZone)
        {
            if (targetZone == startZone)
            {
                Finished = true;
                return;
            }

            _targetZone = targetZone;
            _startZone = startZone;
            _unit = unit;
            _travelPath = GameFrame.Game.ZonePathfinder.GetPath(_startZone, _targetZone);
        }

        public override void Update(float elapsed)
        {

            if (!_started)
            {
                if (_unit.Zone != _startZone)
                    throw new Exception("Cant start moving from a zone which the unit is not in");

                _nextZoneChange = elapsed + ZoneTravelTime;
                _started = true;
            }

            if (_unit.Zone == _targetZone)
            {
                Finished = true;
                // in case of unit being in target zone when action is made (see constructor)
                // continuing with a travelpath containing 0 nodes would throw an exception
                return;
            }

            if (elapsed >= _nextZoneChange)
            {
                _nextZoneChange = elapsed + ZoneTravelTime;
                _unit.EnterZone(_travelPath.First());
                _travelPath.RemoveAt(0);
            }
        }
    }
}