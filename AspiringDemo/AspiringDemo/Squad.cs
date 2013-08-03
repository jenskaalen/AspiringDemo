using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<Unit> Members { get; set; }
        public Faction Faction { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public SquadState State { get; set; }
        public Unit Leader { get; set; }
        public int KillCounter { get; set; }
        //TODO: Rework this? A squad can potentially be in several zones...
        public Zone Zone { get; set; }
        public bool IsVisible { get; set; }

        public void CheckZone()
        { 
        }

        public void IsZoneHostile()
        { 
        }

        public void AddMember(Unit member)
        {
            if (Members == null)
                Members = new List<Unit>();

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

        internal void ChangeRank(Unit member)
        {
            if (Leader == null || member.Rank > Leader.Rank)
                Leader = member;
        }

        internal Squad()
        { 
        
        }
    }
}
