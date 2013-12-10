using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;

namespace AspiringDemo.GameActions.Movement
{
    [Serializable]
    public class EnterInterior : GameAction
    {
        private IUnit _unit;
        private IZone _zone;

        public EnterInterior(IUnit unit, IZone enterZone)
        {
            _unit = unit;
            _zone = enterZone;
        }

        public override void Update(float elapsed)
        {
            _unit.EnterZone(_zone);
            Finished = true;
        }
    }
}
