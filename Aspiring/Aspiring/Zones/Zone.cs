using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Sites;

namespace AspiringDemo.Zones
{
    public enum ZoneType
    {
        Plains,
        Mountains
    }

    //testcomment for git's sake

    public class Zone : IZone
    {
        public Zone(int xPosition, int yPosition, int height, int width)
        {
            PopulatedAreas = new List<IPopulatedArea>();
            Units = new List<IUnit>();
            Neighbours = new List<IPathfindingNode>();
            Area = new Rect(xPosition, yPosition, height, width);
        }

        public Rect Area { get; set; }
        public bool IsPlayerNearby { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public ZoneType Type { get; set; }
        public IEnumerable<IPathfindingNode> Neighbours { get; set; }
        public IPathfindingNode Parent { get; set; }
        public List<IPopulatedArea> PopulatedAreas { get; set; }

        public Vector2 Position
        {
            get { return Area.Center; }
            set { throw new NotImplementedException(); }
        }

        public float GValue { get; set; }
        public float HValue { get; set; }
        public float FValue { get; set; }
        public NodeState State { get; set; }
        public List<IUnit> Units { get; set; }
        public List<IZoneEntrance> ZoneEntrances { get; set; }

        public void AddArea(IPopulatedArea area)
        {
            if (PopulatedAreas == null)
                PopulatedAreas = new List<IPopulatedArea>();

            PopulatedAreas.Add(area);
            area.Zone = this;
        }

        public void AddNeighbour(IZone zone)
        {
            if (Neighbours == null)
                Neighbours = new List<IZone>();

            List<IZone> newNeighbours = Neighbours.Cast<IZone>().ToList();
            newNeighbours.Add(zone);

            Neighbours = newNeighbours;
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