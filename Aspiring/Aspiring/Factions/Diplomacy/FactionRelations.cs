using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Factions.Diplomacy
{
    public class FactionRelations : IFactionRelations
    {
        private List<IFactionRelation> _relations;
        private IFaction _faction;

        public FactionRelations(IFaction faction)
        {
            _faction = faction;
            _relations = new List<IFactionRelation>();
            _relations.Add(new FactionRelation(faction)
                {
                    Relation = RelationType.Friendly
                });
        }

        public List<IFaction> Allies {
            get { return _relations.Where(relation => relation.Relation == RelationType.Friendly).Select(relation => relation.Faction).ToList(); }
        }

        public void SetRelation(IFaction faction, RelationType relation)
        {
            if (faction == _faction)
                throw new NotImplementedException("A faction cant be set a relation to itself");

            if (_relations == null)
                _relations = new List<IFactionRelation>();

            IFactionRelation rel;

            rel = _relations.FirstOrDefault(facRel => facRel.Faction == faction);

            if (rel == null)
            {
                rel = new FactionRelation(faction);
                rel.Relation = relation;
                _relations.Add(rel);
            }

            rel.Relation = relation;
        }

        public IFactionRelation GetRelation(IFaction faction)
        {
            if (_relations == null)
                _relations = new List<IFactionRelation>();

            //forgive me, for i have sinned
            IFactionRelation rel = _relations.FirstOrDefault(facRel => facRel.Faction == faction);

            if (rel == null)
            {
                rel = new FactionRelation(faction);
                _relations.Add(rel);
            }

            return rel;
        }

        public static bool ContainsHostileFactions(IEnumerable<IFaction> factions)
        {
            var enumerable = factions as IList<IFaction> ?? factions.ToList();

            foreach (var relation in enumerable.Select(faction => faction.Relations))
            {
                foreach (var faction in enumerable)
                {
                    if (relation.GetRelation(faction).Relation == RelationType.Hostile)
                        return true;
                }
            }

            return false;
        }
    }
}
