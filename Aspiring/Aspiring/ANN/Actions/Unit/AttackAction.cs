using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Orders;
using AspiringDemo.Units;

namespace AspiringDemo.ANN.Actions.Unit
{
    public class AttackAction
    {
        // change this to a settable value..
        private readonly int _minAttackSize = 1;

        public List<ISquad> Squads { get; set; }
        public IZone AttackTargetZone { get; set; }
        public IZone GatherZone { get; set; }
        public bool AttackStarted { get; set; }
        private readonly IFaction _faction;

        public int MemberCount
        {
            get
            {
                return Squads.Sum(squad => squad.Members.Count);
            }
        }

        public AttackAction(IFaction faction, IZone targetZone, IZone gatherZone, int minUnitsInAttack)
        {
            if (targetZone == null || gatherZone == null)
                throw new Exception("Zones cant be null");

            AttackTargetZone = targetZone;
            Squads = new List<ISquad>();
            GatherZone = gatherZone;
            _faction = faction;
            _minAttackSize = minUnitsInAttack;
        }
        public void AddSquad(ISquad squad)
        {
            Squads.Add(squad);
        }

        public void Work()
        {
            if (AttackStarted)
                return;

            var emptySquads = Squads.Where(squad => !squad.Members.Any());

            foreach (var emptySquad in emptySquads.ToList())
            {
                Squads.Remove(emptySquad);
            }

            if (MemberCount < _minAttackSize)
            {
                // find a squad
                ISquad squad = _faction.Army.Squads.FirstOrDefault(squad1 => squad1.State == SquadState.Idle && !Squads.Contains(squad1));

                if (squad != null)
                {
                    Squads.Add(squad);
                    Work();
                }
                else
                    return;
            }
            else
            {
                var squadsNotInGatherZone = Squads.Where(squad => squad.Members.Any(unit => unit.Zone != GatherZone)).ToList();

                if (squadsNotInGatherZone.Any())
                {
                    foreach (var squad in squadsNotInGatherZone)
                    {
                        // give order to travel to  gatherzone
                        TravelOrder.GiveTravelOrder(squad, GatherZone, true);
                    }
                }
                else
                {
                    //everyone is here - get ready to boogiemove to attack
                    foreach (var squad in Squads)
                    {
                        TravelOrder.GiveTravelOrder(squad, AttackTargetZone, false);
                        squad.Members.ForEach(unit => unit.Order.Execute());
                    }

                    AttackStarted = true;
                }
            }
        }

        public bool IsEveryoneInGatherZone()
        {
            if (MemberCount < _minAttackSize)
                return false;

            bool areAlLSquadsInGatherZone = Squads.All(squad => squad.Members.All(unit => unit.Zone == GatherZone));

            return areAlLSquadsInGatherZone;
        }

        private void StartAttack()
        {
            
        }
    }
}
