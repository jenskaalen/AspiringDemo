//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.Core.Common.CommandTrees;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using AspiringDemo.Factions.Diplomacy;
//using AspiringDemo.GameActions.Combat;
//using AspiringDemo.Units;
//using AspiringDemo.Units.Actions;

//namespace AspiringDemo.Combat
//{
//    /// <summary>
//    /// An AI fight - not the same as a fight where a player is involved
//    /// </summary>
//    public class Fight : IFight
//    {
//        public List<IUnit> FightingUnits {
//            get { return _allUnits.Where(unit => unit.State != UnitState.Dead).ToList(); }
//        }

//        public delegate void FightCleanUpListener();
//        public bool FightActive { get; set; }
//        public int RoundsOfFighting { get; set; }
//        public int FightersCount
//        {
//            get { return _allUnits.Count; }
//        }

//        public FightCleanUpListener FightEnded;
//        private List<IUnit> _allUnits;

//        public Fight()
//        {
//            _allUnits = new List<IUnit>();
//            FightActive = true;
//            FightEnded += CleanFight;
//        }

//        public void PerformFightRound()
//        {
//            foreach (var faction in FightingUnits.Select(unit => unit.Faction).Distinct())
//                Fleeing.CheckAndPerformFleeing(faction, this);

//            List<IUnit> attackersByOrder = GetAttackersByOrder();

//            foreach (IUnit member in attackersByOrder)
//            {
//                if (member.State != UnitState.Dead)
//                {
//                    var target = GetTarget(member, attackersByOrder);

//                    if (target == null) continue;

//                    var attack = new UnitAttack(member, target);
//                    attack.Update(GameFrame.Game.GameTime.Time);
//                }
//            }

//            var fitForFightFactions = _allUnits.Where(x => x.State != UnitState.Dead).Select(x => x.Faction).Distinct();

//            bool containsHostileFactions = FactionRelations.ContainsHostileFactions(fitForFightFactions);

//            if (!containsHostileFactions)
//            {
//                FightEnded();
//            }

//            RoundsOfFighting++; 
//        }

//        public List<IUnit> GetViableTargets(IUnit unit)
//        {
//            List<IUnit> viableTargets = _allUnits.Where(target => target.State != UnitState.Dead && unit.Faction.Relations.GetRelation(target.Faction).Relation == RelationType.Hostile).ToList();

//            return viableTargets;
//        }

//        public void AddUnit(IUnit unit)
//        {
//            if (!_allUnits.Contains(unit) && unit.State != UnitState.Dead)
//            {
//                _allUnits.Add(unit);
//                unit.State = UnitState.Fighting;
//            }
//        }
        
//        private void CleanFight()
//        {
//            //TODO: Move this behaviour unto the units themselves?
//            _allUnits.Where(x => x.State != UnitState.Dead).ToList().ForEach(ch => ch.State = UnitState.Idle);
//            FightActive = false;
//        }

//        private List<IUnit> GetAttackersByOrder()
//        {
//            return _allUnits.OrderBy(x => x.Stats.Speed).ToList();
//        }

//        private IUnit GetTarget(IUnit member, List<IUnit> attackersByOrder)
//        {
//            IUnit target;

//            List<IUnit> viableTargets = GetViableTargets(member);

//            if (viableTargets.Count > 0)
//            {
//                int randomifier = GameFrame.Random.Next(0, viableTargets.Count - 1);
//                target = viableTargets[randomifier];
//            }
//            else
//                target = null;

//            return target;
//        }

//        public void LeaveFight(IUnit unit)
//        {
//            if (_allUnits.Contains(unit))
//                _allUnits.Remove(unit);
//        }
//    }
//}
