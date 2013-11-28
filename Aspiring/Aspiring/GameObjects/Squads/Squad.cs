using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.GameObjects.Squads
{
    public class Squad : ISquad
    {
        public Squad()
        {
            Members = new List<IUnit>();
        }

        public virtual List<IUnit> Members { get; set; }
        //public IFaction Faction { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public SquadState State { get; set; //get { return CurrentState(); }
            //set { throw new NotImplementedException(); } 
        }

        public IUnit Leader { get; set; }
        public int KillCounter { get; set; }
        public bool IsVisible { get; set; }

        public void CheckZone()
        {
            throw new NotImplementedException();
        }

        public void IsZoneHostile()
        {
            throw new NotImplementedException();
        }

        public void AddMember(IUnit member)
        {
            if (Members == null)
                Members = new List<IUnit>();

            Members.Add(member);
            member.ChangeRank += ChangeRank;
            member.Squad = this;

            if (Leader == null || member.Rank > Leader.Rank)
                Leader = member;
        }

        //TODO: Write test for this
        public void RemoveMember(IUnit member)
        {
            Members.Remove(member);
            UpdateMembers();
        }

        public void MemberChangedState(IUnit unit, UnitState state)
        {
            if (!Members.Contains(unit))
                return;

            switch (state)
            {
                case UnitState.Dead:
                    //set the leader in case there has been a change in highest ranking, 
                    SetMostQualifiedLeader();
                    RemoveMember(unit);

                    if (Members.Count(x => x.State == UnitState.Dead) == Members.Count)
                    {
                        // squad is destroyed
                        State = SquadState.Destroyed;
                        //TODO: add faction to squad - this is not very efficient
                        IFaction squadFaction =
                            GameFrame.Game.Factions.FirstOrDefault(faction => faction.Army.Squads.Contains(this));

                        squadFaction.Army.Squads.Remove(this);
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.Fighting:
                    if (Members.Count(x => x.State == UnitState.Fighting) == Members.Count)
                    {
                        State = SquadState.Fighting;
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.ExecutingOrder:
                    if (Members.Count(x => x.State == UnitState.ExecutingOrder) == Members.Count)
                    {
                        State = SquadState.ExecutingOrder;
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.Idle:
                    if (Members.Where(x => x.State == UnitState.Idle).Count() == Members.Count)
                    {
                        State = SquadState.Idle;
                    }
                    else
                        State = SquadState.Mixed;
                    break;
            }
        }

        public void EnterZone(IZone zone)
        {
            foreach (IUnit member in Members)
            {
                member.EnterZone(zone);
            }
        }


        public void UpdateMembers()
        {
            IUnit unit = Members.FirstOrDefault();

            if (unit == null)
                return;

            UnitState state = unit.State;

            switch (state)
            {
                case UnitState.Dead:
                    //set the leader in case there has been a change in highest ranking, 
                    SetMostQualifiedLeader();
                    RemoveMember(unit);

                    if (Members.Count(x => x.State == UnitState.Dead) == Members.Count)
                    {
                        // squad is destroyed
                        State = SquadState.Destroyed;
                        //TODO: add faction to squad - this is not very efficient
                        IFaction squadFaction =
                            GameFrame.Game.Factions.FirstOrDefault(faction => faction.Army.Squads.Contains(this));

                        squadFaction.Army.Squads.Remove(this);
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.Fighting:
                    if (Members.Count(x => x.State == UnitState.Fighting) == Members.Count)
                    {
                        State = SquadState.Fighting;
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.ExecutingOrder:
                    if (Members.Count(x => x.State == UnitState.ExecutingOrder) == Members.Count)
                    {
                        State = SquadState.ExecutingOrder;
                    }
                    else
                        State = SquadState.Mixed;
                    break;

                case UnitState.Idle:
                    if (Members.Where(x => x.State == UnitState.Idle).Count() == Members.Count)
                    {
                        State = SquadState.Idle;
                    }
                    else
                        State = SquadState.Mixed;
                    break;
            }
        }

        internal void ChangeRank(IUnit member, SquadRank rank)
        {
            if (Leader == null || rank > Leader.Rank)
                Leader = member;
        }

        private void SetMostQualifiedLeader()
        {
            IUnit membor = Members.Where(x => x.State != UnitState.Dead).OrderByDescending(x => x.Rank).FirstOrDefault();

            if (Leader != membor)
                Leader = membor;
        }

        //private SquadState CurrentState()
        //{
        //    foreach (UnitState state in Enum.GetValues(typeof(UnitState)))
        //    {
        //        if (Members.All(member => member.State == state))
        //            return (SquadState) Enum.Parse(typeof(SquadState), state.ToString());
        //    }

        //    return SquadState.Mixed;

        //    //if (Members.All(member => member.State == UnitState.ExecutingOrder))
        //    //{
        //    //    return SquadState.ExecutingOrder;
        //    //}
        //    //else if (Members.All(member => member.State == UnitState.Idle))
        //    //{
        //    //    return SquadState.Idle;
        //    //}
        //}
    }
}