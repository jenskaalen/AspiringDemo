using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Orders
{
    public class TravelToZone : ICharacterOrder
    {
        public Character Character { get; set; }
        public bool IsExecuting { get; set; }
        public bool IsDone { get; set; }
        public Zone TargetZone { get; set; }
        public List<Zone> TravelPath { get; set; }

        private Zone _startZone = null;
        private long _nextWorkTime;

        public void Execute()
        {
            Debug.WriteLine("Order initiated");
            if (Character == null)
                throw new Exception("Cant execute without character set");

            if (TargetZone == null)
                throw new Exception("TargetZone cannot be null");

            Character.State = CharacterState.ExecutingOrder;
            _startZone = Character.Zone;
            IsExecuting = true;
            IsDone = false;
            TravelPath = Game.ZonePathfinder.GetPath(_startZone, TargetZone);
            _nextWorkTime = Game.TimeToTravelThroughZone;
        }

        public void Work()
        {
            if (Character.Zone == TargetZone)
            {
                OrderAccomplished();
                return;
            }

            if (Character.State == CharacterState.ExecutingOrder)
            {

                if (_nextWorkTime > Game.GameTime)
                {
                    Character.Zone = TravelPath.First();
                    TravelPath.Remove(Character.Zone);
                    _nextWorkTime += Game.TimeToTravelThroughZone;
                }
            }
        }

        public void OrderAccomplished()
        {
            Character.Order = null;

            if (Character.State == CharacterState.ExecutingOrder)
                Character.State = CharacterState.Idle;
        }
    }
}
