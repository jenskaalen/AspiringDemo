using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions.Diplomacy
{
    public interface IFactionRelation
    {
        IFaction Faction { get; }
        RelationType Relation { get; set; }
    }
}
