using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    /// <summary>
    /// an AI fight
    /// </summary>
    public class Fight
    {
        public delegate void FightCleanUpListener();
        //public List<Squad> Squads { get; private set; }
        public int KilledCount { get; set; }
        public bool FightActive { get; set; }
        public int RoundsOfFighting { get; set; }
        public int FightersCount
        {
            get { return _allUnits.Count; }
        }
        public FightCleanUpListener fightCleanup;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //private List<Squad> _attackReadySquads;
        //TODO: Implement`?=
        private List<Unit> _allUnits;
        private List<Squad> _allSquads;
        private Random _random;

        public Fight()
        {
            _allUnits = new List<Unit>();
            _allSquads = new List<Squad>();
            FightActive = true;
        }

        public void PerformFightRound()
        {
            _random = new Random(DateTime.Now.Millisecond);

            List<Unit> attackersByOrder = GetAttackersByOrder();
            //_allUnits = attackersByOrder;

            foreach (Unit member in attackersByOrder)
            {
                if (member.State != CharacterState.Dead)
                    PerformAttack(member, attackersByOrder);
            }

            //int fitForFightFactions = _allUnits
            //    .Where(x => x.Members.Where(member => member.State != CharacterState.Dead).Any())
            //    .Count();

            int fitForFightFactions = _allUnits.Where(x => x.State != CharacterState.Dead).Select(x => x.Faction).Distinct().Count();

            // if there are 1 or less fighting factions - end fight
            if (fitForFightFactions < 2)
            {
                FightActive = false;
                CleanFight();
            }

            RoundsOfFighting++;
        }

        // cleans up after the fight
        public void CleanFight()
        {
            _allUnits.Where(x => x.State != CharacterState.Dead).ToList().ForEach(ch => ch.State = CharacterState.Idle);
        }

        private void PerformAttack(Unit member, List<Unit> attackersByOrder)
        {
            Unit target = GetTarget(member, attackersByOrder);

            if (target == null)
                return;

            Action action = new Action()
            {
                HPModifier = (-member.Damage)
            };

            target.ApplyAction(action);
        }

        private List<Unit> GetViableTargets(Unit unit)
        {
            List<Unit> viableTargets = new List<Unit>();
            viableTargets = _allUnits.Where(x => x.State != CharacterState.Dead && x.Faction != unit.Faction).ToList();
            
            return viableTargets;
        }

        private Unit GetTarget(Unit member, List<Unit> attackersByOrder)
        {
            Unit target;

            List<Unit> viableTargets = GetViableTargets(member);

            if (viableTargets.Count > 0)
            {
                int randomifier = _random.Next(0, viableTargets.Count - 1);
                target = viableTargets[randomifier];
            }
            else
                target = null;

            return target;
        }

        private List<Unit> GetAttackersByOrder()
        {
            return _allUnits.OrderBy(x => x.Speed).ToList();
        }

        //private List<Unit> GetAttackersByOrder()
        //{
        //    List<Unit> attackersByOrder = new List<Unit>();

        //    foreach (Squad squad in _allSquads)
        //    {
        //        squad.Members.Where(x => x.State != CharacterState.Dead).ToList().ForEach(member => attackersByOrder.Add(member));
        //    }

        //    attackersByOrder = attackersByOrder.OrderByDescending(x => x.Speed).ToList();

        //    return attackersByOrder;
        //}

        public void GetSquadToAttack(Squad attackingSquad)
        {
            List<Squad> enemySquads = _allSquads.Where(x => x.State == SquadState.Fighting && x.Faction != attackingSquad.Faction).ToList();

            if (enemySquads.Count == 0)
                throw new Exception("No enemy squads found in zone");

            Squad squadToAttack = enemySquads[_random.Next(0, enemySquads.Count - 1)];
        }

        public void AddUnit(Unit unit)
        {
            if (!_allUnits.Contains(unit))
            {
                _allUnits.Add(unit);
                unit.State = CharacterState.Fighting;
            }
        }

        public void AddSquad(Squad squad)
        {
            if (!_allSquads.Contains(squad))
                _allSquads.Add(squad);

            foreach (Unit member in squad.Members)
            {
                if (!_allUnits.Contains(member))
                    _allUnits.Add(member);
            }
        }

        public void RemoveSquad(Squad squad)
        {
            throw new NotImplementedException();
        }

        //TODO: Remove
        //public Squad GetAttackingSquad()
        //{
        //    Squad attackingSquad = _attackReadySquads.OrderByDescending(x => x.Members.Average(sq => sq.Speed)).FirstOrDefault();
            

        //    return attackingSquad;
        //}

    }
}
