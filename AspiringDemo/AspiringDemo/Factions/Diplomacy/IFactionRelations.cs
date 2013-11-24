using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions.Diplomacy
{
    public interface IFactionRelations
    {
        void SetRelation(IFaction faction, RelationType relation);
        IFactionRelation GetRelation(IFaction faction);
    }
}
