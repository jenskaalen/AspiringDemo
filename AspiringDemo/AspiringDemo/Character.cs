using AspiringDemo.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public enum CharacterState
    {
        Idle,
        Fighting,
        ExecutingOrder,
        Dead
    }


    public class Character
    {
        public delegate ActionResult ActionApplied(Action action);
        public delegate void StateChanged(CharacterState state);

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int HP { get; set; }
        public int Speed { get; set; }
        public int Toughness { get; set; }
        public int Kills { get; protected set; }
        public int Damage { get; set; }
        public CharacterState State 
        { 
            get
            {
                return _state;
            }
            set
            {
                ChangeState(value);
            }
        }
        public ActionApplied ApplyAction;
        public StateChanged ChangeState;
        public ICharacterOrder Order { get; set; }
        public Zone Zone { get; set; }

        private CharacterState _state;

        internal Character()
        {
            ApplyAction += ApplyActionOnSelf;
            ChangeState += ChangeStateSelf;
            _state = CharacterState.Idle;
        }

        private void ChangeStateSelf(CharacterState state)
        {
            _state = state;
        }

        private ActionResult ApplyActionOnSelf(Action action)
         {
            HP += action.HPModifier;
            bool killed = false;

            if (HP < 1)
            {
                State = CharacterState.Dead;
                killed = true;
            }

            ActionResult result = new ActionResult();
            result.KilledTarget = killed;

            return result;
        }
    }
}
