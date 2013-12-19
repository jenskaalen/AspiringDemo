using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Factions;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Zones.Interiors;

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
            Creatures = new List<IUnit>();

            for (int unitsCreated=0; unitsCreated < _unitCount; unitsCreated++)
            {
                CreateCreature();
            }
        }

        public void Populate(IInterior interior)
        {
            var occupiedNodes = new List<IPathfindingNode>();
            var placedUnits = new List<IUnit>();
            int unitsPerRoom = _unitCount / interior.Rooms.Count;

            foreach (var room in interior.Rooms)
            {
                //TODO: oh my god this is slow - optimize it
                Room room1 = room;
                var roomNodes = interior.Nodes.Where(node => room1.Contains(node.Position)).ToArray();

                for (int i = 0; i < unitsPerRoom; i++)
                {
                    var spot = roomNodes[GameFrame.Random.Next(0, roomNodes.Length - 1)];
                    //var spot = roomNodes.First(node => !occupiedNodes.Contains(node));
                    occupiedNodes.Add(spot);
                    var unitToPlace = Creatures.FirstOrDefault(creature => !placedUnits.Contains(creature));
                    PlaceCreature(unitToPlace, interior, spot.Position);
                    placedUnits.Add(unitToPlace);
                }
            }
        }

        private void PlaceCreature(IUnit unit, IInterior interior, Vector2 position)
        {
            unit.EnterZone(interior);
            unit.Position = position;
        }

        private void CreateCreature()
        {
            var unit = _faction.Create<Zombie>();
            Creatures.Add(unit);
        }

        //private void CreateCreature(IInterior interior, Vector2 position)
        //{
        //    var unit = _faction.Create<Zombie>();
        //    unit.EnterInterior(interior);
        //    unit.Position = position;
        //    Creatures.Add(unit);
        //}
    }
}
