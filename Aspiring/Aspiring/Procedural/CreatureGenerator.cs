using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Procedural
{
    public class CreatureGenerator : ICreatureGenerator
    {
        public List<GameObjects.Units.IUnit> Creatures
        {
            get; private set;
        }

        public virtual void Populate()
        {
            throw new NotImplementedException();
        }
    }
}
