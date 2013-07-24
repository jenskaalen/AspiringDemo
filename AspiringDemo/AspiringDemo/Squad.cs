using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public enum SquadState 
    {
        Idle,
        Fighting,
        Fleeing,
        Destroyed
    }

    public class Squad
    {
        public List<SquadMember> Members { get; private set; }
        public Faction Faction { get; set; }
        public Guid ID { get; set; }
        public SquadState State { get; set; }
        public SquadMember Leader { get; private set; }
        public int KillCounter { get; private set; }
        public Zone Zone { get; set; }
        public bool IsVisible { get; set; }

        public void CheckZone()
        { 
        }

        public void IsZoneHostile()
        { 
        }

        public void AddMember(SquadMember member)
        {
            if (Members == null)
                Members = new List<SquadMember>();

            member.Squad = this;
            Members.Add(member);
            ChangeRank(member);
            member.ChangeState += MemberChangedState;
        }

        private void MemberChangedState(CharacterState state)
        {
            if (state == CharacterState.Dead)
            {
                if (Members.Where(x => x.State == CharacterState.Dead).Count() == Members.Count)
                { 
                    // squad is destroyed
                    State = SquadState.Destroyed;
                }
            }
        }

        internal void ChangeRank(SquadMember member)
        {
            if (Leader == null || member.Rank > Leader.Rank)
                Leader = member;
        }
    }
}
