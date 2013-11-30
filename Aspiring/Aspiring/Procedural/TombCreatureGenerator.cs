using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Procedural.Interiors;

namespace AspiringDemo.Procedural
{
    public class TombCreatureGenerator : CreatureGenerator
    {
        private int _unitCount;
        private IFaction _faction;

        public TombCreatureGenerator(int unitCount, IFaction faction)
        {
            _unitCount = unitCount;
            _faction = faction;
        }

        public void Populate(IInterior interior)
        {
            var occupiedNodes = new List<IPathfindingNode>();
            int unitsPerRoom = interior.Rooms.Count /_unitCount;

            foreach (var room in interior.Rooms)
            {
                //TODO: oh my god this is slow - optimize it
                Room room1 = room;
                var roomNodes = interior.InteriorNodes.Where(node => room1.Contains(node.Position));

                int unitsCreated = 0;
                while (unitsCreated < unitsPerRoom)
                {
                    // pick a random spot which is not occupied
                    var spot = roomNodes.FirstOrDefault(node => !occupiedNodes.Contains(node));

                    occupiedNodes.Add(spot);
                }
            }
        }

        private void CreateCreature()
        {
            _faction.Create<Zombie>();
        }

        private void CreateCreature(IInterior interior, Vector2 position)
        {
            _faction.Create<Zombie>();
        }
    }
}
