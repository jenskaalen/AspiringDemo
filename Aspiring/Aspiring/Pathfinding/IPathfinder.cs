using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Pathfinding
{
    public interface IPathfinder<T> where T : IPathfindingNode
    {
        List<T> Nodes { get; set; }
        // should these be properties?
        //List<IPathfindingNode> ClosedNodes { get; set; }
        //List<IPathfindingNode> OpenNodes { get; set; }

        List<T> GetPath(Vector2 startPosition, Vector2 endPosition);
    }
}
