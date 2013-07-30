using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class DemoSquad
    {
        public List<Unit> Members { get; set; }
        public Faction Faction { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public SquadState State { get; set; }
        public Unit Leader { get; set; }
        public int KillCounter { get; set; }
        public Zone Zone { get; set; }
        public bool IsVisible { get; set; }

        public void CheckZone()
        {
        }

        public void IsZoneHostile()
        {
        }

        internal void ChangeRank(Unit member)
        {
            if (Leader == null || member.Rank > Leader.Rank)
                Leader = member;
        }
    }
}
