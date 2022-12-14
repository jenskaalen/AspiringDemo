using AspiringDemo.Gamecore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Pathfinding
{
    [Serializable]
    public class Pathfinder<T> : IPathfinder<T> where T : class, IPathfindingNode, IComparable<T>
    {
        public PriorityQueue<T> OpenList { get; set; }
        //public SortedPath<T> OpenList { get; set; }
        //public SortedSet<T> OpenList { get; set; }
        //public List<T> OpenList { get; set; }
        public List<T> ClosedList { get; set; }
        public List<T> Nodes { get; set; }


        public List<T> GetPath(T startNode, T endNode)
        {
            if (startNode == null || endNode == null)
                throw new Exception("Startnode or endnode cant be null");

            OpenList = new PriorityQueue<T>();
            //OpenList = new SortedPath<T>();
            //OpenList = new List<T>();
            //OpenList = new SortedSet<T>();
            ClosedList = new List<T>();

            OpenList.Put(startNode);

            while (true)
            {
                if (OpenList.data.Count == 0)
                    return null;
                //just for testing - this is very slow
                T currentNode = OpenList.Pop();
                //T currentNode = OpenList.First();
                //OpenList.Remove(OpenList.First());

                ClosedList.Add(currentNode);

                if (currentNode == endNode)
                {
                    // end

                    return BacktraceParents(currentNode, startNode);
                }

                AssessNode(currentNode, endNode);
            }
        }

        /// <summary>
        ///     Finds nodes and adds them to OpenList if viable for the path. If node already is in OpenList - f,g,h values of node
        ///     are re-computed
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="endNode"></param>
        private void AssessNode(T currentNode, T endNode)
        {
            if (!currentNode.Neighbours.Any())
                throw new Exception("No neighbour nodes!");

            int anons = 0;

            foreach (T nNode in currentNode.Neighbours)
            {
                if (nNode.State == NodeState.Closed)
                    continue;


                if (ClosedList.Contains(nNode))
                {
                    continue;
                }

                if (!nNode.Neighbours.Any() && nNode != endNode)
                {
                    continue;
                }

                // seemingly makes no difference
                //float gCost = (float) Utility.GetDistance(currentNode.Position, nNode.Position) + currentNode.GValue;
                float gCost = 0.5f + currentNode.GValue;
                //float gCost = currentNode.DistanceToNode(nNode);

                //if (!OpenList.ContainsNode(nNode))
                if (!OpenList.Contains(nNode))
                {
                    nNode.GValue = gCost;
                    nNode.HValue = (Math.Abs(endNode.Position.X - nNode.Position.X) +
                                   Math.Abs(endNode.Position.Y - nNode.Position.Y));
                    //+ Math.Abs(endNode.Position.z - nNode.Position.z);
                    nNode.FValue = nNode.GValue + nNode.HValue;
                    nNode.Parent = currentNode;
                    OpenList.Put(nNode);
                }
                else
                {
                    //nNode.GValue = gCost;
                    //nNode.FValue = gCost + nNode.HValue;
                    //nNode.Parent = currentNode;

                    if (nNode.GValue > gCost)
                    {
                        nNode.GValue = gCost;
                        nNode.FValue = gCost + nNode.HValue;
                        nNode.Parent = currentNode;

                        //"refresh" prioriotyqueue. This might need a rework
                        //OpenList.Pop(nNode);
                        OpenList.data.Remove(nNode);
                        OpenList.Put(nNode);

                    }
                }

                if (!ClosedList.Contains(nNode) && !OpenList.Contains(nNode))
                    anons++;
            }
        }

        private List<T> BacktraceParents(T node, T startNode)
        {
            var tracedNodes = new List<T>();

            while (node != startNode)
            {
                tracedNodes.Add(node);
                node = (T)node.Parent;
            }
            // does not add last node to path
            tracedNodes.Reverse();
            return tracedNodes;
        }


        public IPathfindingNode GetClosestNode(Vector2 position)
        {
            //NOTE: inefficient, slow etc
            var node = Nodes.OrderBy(x => GetDistance(position, x.Position)).First();
            return node;
        }


        private static double GetDistance(Vector2 position1, Vector2 position2)
        {
            //double dist = Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
            double dist = Math.Sqrt((position1.X - position2.X) * (position1.X - position2.X) +
                                    (position1.Y - position2.Y) * (position1.Y - position2.Y));
            return dist;
        }

        public void SetNeighbours(int width, int height)
        {
            foreach (var node in Nodes)
            {
                node.Neighbours = Neighbours(node, width, height);
            }
        }


        private List<T> Neighbours(T checkzone, int width, int height)
        {
            var neighbours = new List<T>();

            int xPos = checkzone.Position.X;
            int yPos = checkzone.Position.Y;

            var zone1 = Nodes.FirstOrDefault(x => x.Position.X == (xPos - width) && x.Position.Y == yPos);
            var zone2 = Nodes.FirstOrDefault(x => x.Position.X == (xPos + width) && x.Position.Y == yPos);
            var zone3 = Nodes.FirstOrDefault(x => x.Position.Y == (yPos - height) && x.Position.X == xPos);
            var zone4 = Nodes.FirstOrDefault(x => x.Position.Y == (yPos + height) && x.Position.X == xPos);

            if (zone1 != null)
                neighbours.Add(zone1);

            if (zone2 != null)
                neighbours.Add(zone2);

            if (zone3 != null)
                neighbours.Add(zone3);

            if (zone4 != null)
                neighbours.Add(zone4);

            return neighbours;
        }

        //private void AddChange(IPathfindingNode node, NodeType type)
        //{
        //    MainWindow.Changes.Add(new NodeChange()
        //    {
        //        Rect = ((Node)node).Rect,
        //        Type = type,
        //        Node = (Node)node
        //    });
        //}


        public List<T> GetPath(Gamecore.Types.Vector2 startPosition, Gamecore.Types.Vector2 endPosition)
        {
            throw new NotImplementedException();
        }
    }
}
