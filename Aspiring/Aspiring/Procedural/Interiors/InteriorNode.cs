﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Pathfinding;

namespace AspiringDemo.Procedural.Interiors
{
    public class InteriorNode : IInteriorNode
    {
        public Vector2 Position { get; set; }

        public IEnumerable<Pathfinding.IPathfindingNode> Neighbours { get; set; }

        public Pathfinding.IPathfindingNode Parent { get; set; }

        public float GValue { get; set; }

        public float HValue { get; set; }

        public float FValue { get; set; }

        public Pathfinding.NodeState State { get; set; }

        public InteriorNode(int xPosition, int yPosition)
        {
            Position = new Vector2(xPosition, yPosition);
        }

        public int CompareTo(IPathfindingNode other)
        {
            if (HValue < other.HValue) return -1;
            if (HValue > other.HValue) return 1;
            return 0;
        }

        public float DistanceToNode(IPathfindingNode targetNode)
        {
            return
                (float)
                    Math.Sqrt(Math.Pow((targetNode.Position.X - Position.X), 2) +
                              Math.Pow((targetNode.Position.Y - Position.Y), 2));
        }
    }
}