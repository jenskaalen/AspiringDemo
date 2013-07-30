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
        public List<Squad> Squads { get; set; }
        public int KilledCount { get; set; }
        public bool FightActive { get; set; }
        public int RoundsOfFighting { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        private List<Squad> _attackReadySquads;
        //TODO: Implement`?=
        private List<Unit> allMembers;
        private Random _random;

        public Fight()
        {
            Squads = new List<Squad>();
            FightActive = true;
        }

        public void PerformFightRound()
        {
            _random = new Random(DateTime.Now.Millisecond);
            _attackReadySquads = Squads;

            //Squad attackingSquad = GetAttackingSquad();
            //_attackReadySquads.Remove(attackingSquad);
            List<Unit> attackersByOrder = GetAttackersByOrder();
            allMembers = attackersByOrder;

            foreach (Unit member in attackersByOrder)
            {
                if (member.State != CharacterState.Dead)
                    PerformAttack(member, attackersByOrder);
            }

            int fitForFightFactions = _attackReadySquads
                .Where(x => x.Members.Where(member => member.State != CharacterState.Dead).Any())
                .Count();

            // if there are 1 or less fighting factions - end fight
            if (fitForFightFactions < 2)
            {
                FightActive = false;
                CleanFight();
            }

            RoundsOfFighting++;
        }

        // cleans up after the fight
        private void CleanFight()
        {
            allMembers.Where(x => x.State != CharacterState.Dead).ToList().ForEach(ch => ch.State = CharacterState.Idle);
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

        private Unit GetTarget(Unit member, List<Unit> attackersByOrder)
        {
            Unit target;

            List<Unit> viableTargets = attackersByOrder.Where(x => x.State != CharacterState.Dead && x.Squad.Faction != member.Squad.Faction).ToList();

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
            List<Unit> attackersByOrder = new List<Unit>();

            foreach (Squad squad in Squads)
            {
                squad.Members.Where(x => x.State != CharacterState.Dead).ToList().ForEach(member => attackersByOrder.Add(member));
            }

            attackersByOrder = attackersByOrder.OrderByDescending(x => x.Speed).ToList();

            return attackersByOrder;
        }

        public void GetSquadToAttack(Squad attackingSquad)
        {
            List<Squad> enemySquads = Squads.Where(x => x.State == SquadState.Fighting && x.Faction != attackingSquad.Faction).ToList();

            if (enemySquads.Count == 0)
                throw new Exception("No enemy squads found in zone");

            Squad squadToAttack = enemySquads[_random.Next(0, enemySquads.Count - 1)];
        }

        public void AddSquad(Squad squad)
        {
            throw new NotImplementedException();
        }

        public void RemoveSquad(Squad squad)
        {
            throw new NotImplementedException();
        }

        public Squad GetAttackingSquad()
        {
            Squad attackingSquad = _attackReadySquads.OrderByDescending(x => x.Members.Average(sq => sq.Speed)).FirstOrDefault();
            return attackingSquad;
        }

    }
}
