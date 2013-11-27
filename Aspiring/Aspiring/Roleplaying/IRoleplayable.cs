using AspiringDemo.Roleplaying.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Roleplaying
{
    public interface IRoleplayable
    {
        IUnitStats Stats { get; set; }
        ICharacterSkills Skills { get; set; }
        ICharacterLevel CharacterLevel { get; set; }
        void Loot(IUnit unit);
    }
}
