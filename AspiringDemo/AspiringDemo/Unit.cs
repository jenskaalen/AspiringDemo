using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspiringDemo
{
    public class Unit : Character
    {
        private SquadRank _rank;

        public SquadRank Rank
        {
            get { return _rank; }
            set 
            { 
                _rank = value;

                if (Squad != null)
                    Squad.ChangeRank(this);
            }
        }
        
        public Squad Squad { get;  set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }

        public Unit()
        {
            Rank = SquadRank.Private;
            Name = "";
            State = CharacterState.Idle;
            Speed = 20;
            Damage = 5;
            HP = 25;
        }

        public void Attack(Unit target)
        { 
            Action action = new Action();
            action.HPModifier = -20;
            ActionResult result = target.ApplyAction(action);

            if (result.KilledTarget)
                Kills++;
        }

        
    }
}
