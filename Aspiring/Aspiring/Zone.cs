using System.Data.Odbc;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.Factions.Diplomacy;
using AspiringDemo.Pathfinding;
using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using AspiringDemo.Units;

namespace AspiringDemo
{
    public enum ZoneType
    {
        Plains,
        Mountains
    }

    public class Zone : IZone
    {
        public int PositionXStart { get; set; }
        public int PositionXEnd { get; set; }
        public int PositionYStart { get; set; }
        public int PositionYEnd { get; set; }
        public bool IsPlayerNearby { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ZoneType Type { get; set; }
        public IEnumerable<IPathfindingNode> Neighbours { get; set; }
        public IPathfindingNode Parent { get; set; }
        public List<IPopulatedArea> PopulatedAreas { get; set; }

        public Vector2 Position
        {
            get { return new Vector2( (PositionXStart + PositionXEnd) / 2, (PositionYEnd + PositionYStart) / 2); }
            set { throw new NotImplementedException(); }
        }

        public float GValue { get; set; }
        public float HValue { get; set; }
        public float FValue { get; set; }
        public NodeState State { get; set; }
        public List<IUnit> Units { get; set; }

        public Zone()
        {
            PopulatedAreas = new List<IPopulatedArea>();
            Units = new List<IUnit>();
            Neighbours = new List<IPathfindingNode>();
        }

        private void AddUnit(IUnit unit)
        {
            if (Units == null)
                Units = new List<IUnit>();

            Units.Add(unit);
        }

        public void AddArea(IPopulatedArea area)
        {
            if (PopulatedAreas == null)
                PopulatedAreas = new List<IPopulatedArea>();

            PopulatedAreas.Add(area);
            area.Zone = this;
        }

        //private void TryRazeZone(IUnit unit)
        //{
        //    //Checks if the losing side had any buildings in the area that will be razed
        //    foreach (IPopulatedArea area in PopulatedAreas)
        //    {
        //        area.IsUnderAttack = false;

        //        area.Razed = area.Owner != unit.Faction;
        //    }
        //}

        private bool IsZoneContested()
        {
            var differentFactions = new HashSet<IFaction>();

            foreach (IUnit unit in Units.Where(unit => unit.State != UnitState.Dead))
                differentFactions.Add(unit.Faction);

            foreach (var faction in differentFactions)
            {
                IFaction faction1 = faction;

                // please forgive me
                bool isHostile =
                    differentFactions.Where(faction2 => faction2 != faction1).Any(
                        relFaction => relFaction.Relations.GetRelation(faction1).Relation == RelationType.Hostile);

                if (isHostile)
                    return true;
            }

            return false;
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

            if (this.HValue < other.HValue) return -1;
            else if (this.HValue > other.HValue) return 1;
            else return 0;
        }

        public float DistanceToNode(IPathfindingNode targetNode)
        {
            return (float) Math.Sqrt(Math.Pow((targetNode.Position.X - this.Position.X), 2) + Math.Pow((targetNode.Position.Y - this.Position.Y), 2));
        }
    }
}
