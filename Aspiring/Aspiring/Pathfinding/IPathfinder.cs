using System.Collections.Generic;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Pathfinding
{
    public interface IPathfinder<T> where T : IPathfindingNode
    {
        List<T> Nodes { get; set; }

        List<T> GetPath(Vector2 startPosition, Vector2 endPosition);
    }
}