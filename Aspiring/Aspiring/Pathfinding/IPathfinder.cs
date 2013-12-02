using System.Collections.Generic;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Pathfinding
{
    //TODO: Extract interface from pathfinder-implementation - bring things up to speed
    public interface IPathfinder<T> where T : IPathfindingNode
    {
        List<T> Nodes { get; set; }
        List<T> GetPath(Vector2 startPosition, Vector2 endPosition);

        /// <summary>
        /// Sets neighbours on every node in the pathfinder based on a set grid. Only works on grids.
        /// </summary>
        void SetNeighbours(int width, int height);
    }
}