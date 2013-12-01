using System;
using System.Collections.Generic;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Pathfinding
{
    public enum NodeState : byte
    {
        Open,
        Closed
    }

    public interface IPathfindingNode : IComparable<IPathfindingNode>
    {
        Vector2 Position { get; set; }
        IEnumerable<IPathfindingNode> Neighbours { get; set; }
        IPathfindingNode Parent { get; set; }
        float GValue { get; set; }
        float HValue { get; set; }
        float FValue { get; set; }

        NodeState State { get; set; }

        float DistanceToNode(IPathfindingNode targetNode);
    }
}