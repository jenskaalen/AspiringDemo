using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore.Helpers;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions.Movement
{
    public class MoveToPosition : GameAction
    {
        private IUnit _unit;
        private Vector2 _position;
        private List<IPathfindingNode> _travelPath;
        private bool _started;
        private IPathfindingNode _targetNode;
        private IPathfindingNode _startNode;
        private float _nextZoneChange;
        //TODO: base movespeed upon unit stats
        private float moveSpeed = 1f;


        public MoveToPosition(IUnit unit, Vector2 position)
        {
            _unit = unit;
            _position = position;
            //NOTE: horribly inefficient :(
            //closest

            var unitNode = unit.Zone.Pathfinder.GetClosestNode(unit.Position);
            var targetNode = unit.Zone.Pathfinder.GetClosestNode(position);

            _targetNode = targetNode;
            _startNode = unitNode;
            _travelPath = unit.Zone.Pathfinder.GetPath(unitNode, targetNode);
        }

        public override void Update(float elapsed)
        {

            if (!_started)
            {
                _nextZoneChange = elapsed + moveSpeed;
                _started = true;
            }

            if (_unit.Position.Equals(_targetNode.Position))
            {
                Finished = true;
                // in case of unit being in target zone when action is made (see constructor)
                // continuing with a travelpath containing 0 nodes would throw an exception
                return;
            }

            if (elapsed >= _nextZoneChange)
            {
                _nextZoneChange = elapsed + moveSpeed;
                _unit.Position = _travelPath.First().Position;
                _travelPath.RemoveAt(0);
            }
        }
    }
}
