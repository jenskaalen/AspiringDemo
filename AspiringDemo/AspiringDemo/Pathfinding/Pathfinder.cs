using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Pathfinding
{
    public class Pathfinder<T> : IPathfinder<T> where T : class, IPathfindingNode, IComparable<T>
    {
        public List<T> Nodes { get; set;  }
        public PriorityQueue<T> OpenList { get; set; }
        public List<T> ClosedList { get; set; }

        public List<T> GetPath(Vector2 startPosition, Vector2 endPosition)
        {
            throw new NotImplementedException();
        }

        public List<T> GetPath(T startNode, T endNode)
        {
            if (startNode == null || endNode == null)
                throw new Exception("Startnode or endnode cant be null");

            List<T> path = new List<T>();
            OpenList = new PriorityQueue<T>();
            ClosedList = new List<T>();

            T currentNode = startNode;
            OpenList.Put(startNode);

            while (true)
            {
                currentNode = OpenList.Pop();

                if (currentNode == endNode)
                { 
                    // end
                    ClosedList.Add(currentNode);

                    return BacktraceParents(currentNode, startNode);
                }

                AssessNode(currentNode, endNode);
            }
        }

        /// <summary>
        /// Finds nodes and adds them to OpenList if viable for the path. If node already is in OpenList - f,g,h values of node are re-computed
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="endNode"></param>
        private void AssessNode(T currentNode, T endNode)
        {
            T bestNode = null;
            double lowestF = double.MaxValue;

            if (!currentNode.Neighbours.Any())
                throw new Exception("No neighbour nodes!");

            foreach (T nNode in currentNode.Neighbours)
            {
                if (nNode.State == NodeState.Closed)
                    continue;

                if (ClosedList.Contains(nNode))
                    continue;

                if (!nNode.Neighbours.Any() && nNode != endNode)
                {
                    continue;
                }

                float gCost = currentNode.DistanceToNode(nNode);

                if (!OpenList.ContainsNode(nNode))
                {
                    nNode.GValue = gCost;
                    nNode.HValue = Math.Abs(endNode.Position.X - nNode.Position.X) + Math.Abs(endNode.Position.Y - nNode.Position.Y ); //+ Math.Abs(endNode.Position.z - nNode.Position.z);
                    nNode.FValue = nNode.GValue + nNode.HValue;
                    nNode.Parent = currentNode;
                    OpenList.Put(nNode);
                }

                else
                {
                    if (nNode.GValue > gCost)
                    {
                        nNode.GValue = gCost;
                        nNode.FValue = gCost + nNode.HValue;
                        nNode.Parent = currentNode;
                    }
                }

                if (nNode.FValue < lowestF)
                {
                    lowestF = nNode.FValue;
                    bestNode = nNode;
                }
            }

            ClosedList.Add(currentNode);
        }

        private List<T> BacktraceParents(T node, T startNode)
        {
            List<T> tracedNodes = new List<T>();

            while (node != startNode)
            {
                tracedNodes.Add(node);
                node = (T) node.Parent;
            }
            // does not add last node to path
            tracedNodes.Reverse();

            return tracedNodes;
        }
    }
}
