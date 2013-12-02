using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Combat.Behaviour
{
    public class Detection : IDetection
    {
        //TODO: Add line-of-sight checking
        public bool DetectEnemy(IUnit searchingUnit, IUnit undetectedUnit)
        {
            //NOTE: possibly just change this to return false
            if (searchingUnit.Zone != undetectedUnit.Zone)
                throw new Exception("Cant attempt to detect a unit that is in a different zone");

            double distance = Utility.GetDistance(searchingUnit.Position, undetectedUnit.Position);

            if (distance < searchingUnit.CombatModule.DetectionDistance)
                return true;
            else 
                return false;
        }
    }
}
