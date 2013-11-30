using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Procedural.Interiors
{
    public class Area : IInterior
    {
        public List<IInteriorNode> InteriorNodes { get; set; }
        public Room Entrance { get; set; }
        public int InteriorWidth { get; set; }
        public int InteriorHeight { get; set; }

        public void Enter(IUnit unit)
        {
            unit.Position = Entrance.Center;
        }

        protected List<Room> Rooms { get; set; }

        public void Populate()
        {
            throw new NotImplementedException();
        }

        public Area()
        {

        }

        protected void CreateSpace()
        {

        }


        List<Room> IInterior.Rooms { get; set; }

        public List<CorridorPath> Paths { get; set; }
    }
}
