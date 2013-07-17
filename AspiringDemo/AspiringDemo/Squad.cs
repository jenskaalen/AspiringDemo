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
        Fleeing
    }

    public class Squad
    {
        public List<SquadMember> Members { private get; set; }
        public Faction Faction { get; set; }
        public Guid ID { get; set; }
        public SquadState State { get; set; }
        public SquadMember Leader { get; private set; }
        public int KillCounter { get; private set; }
        public Zone Zone { get; set; }

        public void CheckZone()
        { 
            
        }

        public void IsZoneHostile()
        { 
            
        }
    }
}
