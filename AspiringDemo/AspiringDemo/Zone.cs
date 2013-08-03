using AspiringDemo.Pathfinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public enum ZoneType
    {
        Plains,
        Mountains
    }

    public class Zone : IPathfindingNode
    {
        public int PositionXStart { get; set; }
        public int PositionXEnd { get; set; }
        public int PositionYStart { get; set; }
        public int PositionYEnd { get; set; }
        public bool IsPlayerNearby { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ZoneType Type { get; set; }
        public Fight Fight { get; set; }
        //public Zone[] Neighbours { get; set; }
        public IEnumerable<IPathfindingNode> Neighbours { get; set; }
        public IPathfindingNode Parent { get; set; }

        public Vector2 Position
        {
            get { return new Vector2( (PositionXStart + PositionXEnd) / 2, (PositionYEnd + PositionYStart) / 2); }
            set { throw new NotImplementedException(); }
        }

        public float GValue { get; set; }
        public float HValue { get; set; }
        public float FValue { get; set; }
        public NodeState State { get; set; }

        public void LoadZone()
        {
        }

        public void UnloadZone()
        {
        }

        public void AddNeighbour(Zone zone)
        {
            if (Neighbours == null)
                Neighbours = new List<Zone>();

            List<Zone> newNeighbours = Neighbours.Cast<Zone>().ToList();
            newNeighbours.Add(zone);

            Neighbours = newNeighbours;
        }


        public int CompareTo(IPathfindingNode other)
        {

            if (this.HValue < other.HValue) return -1;
            else if (this.HValue > other.HValue) return 1;
            else return 0;
        }

        public float DistanceToNode(IPathfindingNode targetNode)
        {
            return (float) Math.Sqrt((targetNode.Position.X - this.Position.X) + (targetNode.Position.Y - this.Position.Y));
        }
    }
}
