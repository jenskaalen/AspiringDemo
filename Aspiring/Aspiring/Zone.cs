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
        public Fight Fight { get; set; }
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

        ///// <summary>
        ///// Adds the unit to the zone and checks if the zone is contested
        ///// </summary>
        ///// <param name="character"></param>
        //public void EnterZone(IUnit unit)
        //{
        //    AddUnit(unit);

        //    //TODO: Remove this
        //    if (unit.Zone != null && unit.Zone != this)
        //        unit.Zone.LeaveZone(unit);

        //    unit.Zone = this;

        //    if (unit.IsPlayer)
        //        IsPlayerNearby = true;

        //    if (Fight != null)
        //    {
        //        unit.State = UnitState.Fighting;
        //        Fight.AddUnit(unit);
        //    }
        //    else
        //    {
        //        bool contested = IsZoneContested();

        //        if (contested)
        //        {
        //            CreateFight();

        //            foreach (IUnit squnit in Units)
        //            {
        //                Fight.AddUnit(squnit);
        //            }

        //            foreach (IPopulatedArea area in PopulatedAreas)
        //            {
        //                area.IsUnderAttack = true;
        //            }
        //        }
        //        else
        //        {
        //            TryRazeZone(unit);
        //        }
        //    }
        //}

        //public void LeaveZone(IUnit unit)
        //{
        //    Units.Remove(unit);

        //    if (!Units.Where(x => x.IsPlayer).Any())
        //        IsPlayerNearby = false;
        //}

        private void CreateFight()
        {
            Fight fight = new Fight();
            this.Fight = fight;
            this.Fight.FightEnded += EndFight;
        }

        private void TryRazeZone(IUnit unit)
        {
            //Checks if the losing side had any buildings in the area that will be razed
            foreach (IPopulatedArea area in PopulatedAreas)
            {
                area.IsUnderAttack = false;

                area.Razed = area.Owner != unit.Faction;
            }
        }

        private void EndFight()
        {
            //TODO: This will have to be reworked to support allied factions
            IUnit anyUnit = Units.FirstOrDefault(x => x.State != UnitState.Dead);

            if (anyUnit == null)
            {
                if (Units.All(unit => unit.State == UnitState.Dead))
                {
                    Fight = null;
                    return;
                }
                else
                {
                    //throw new Exception("No units found in zone after fight - this was not supposed to happen!");
                }
            }

            Fight = null;

            //Checks if the losing side had any buildings in the area that will be razed
            foreach (IPopulatedArea area in PopulatedAreas)
            {
                area.IsUnderAttack = false;

                if (area.Owner == anyUnit.Faction)
                    area.Razed = false;
                else
                    area.Razed = true;
            }
        }

        //TODO: Remove
        //public void EnterZone(ISquad squad)
        //{
        //    squad.Members.ForEach(EnterZone);
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
