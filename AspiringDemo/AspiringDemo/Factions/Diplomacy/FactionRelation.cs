using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions.Diplomacy
{
    public class FactionRelation : IFactionRelation
    {
        public IFaction Faction
        {
            get; private set;
        }

        public RelationType Relation { get; set; }

        public FactionRelation(IFaction faction)
        {
            // TODO: Complete member initialization
            this.Faction = faction;
            Relation  = RelationType.Hostile;
        }
    }
}
