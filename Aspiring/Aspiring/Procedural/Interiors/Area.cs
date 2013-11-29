using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Procedural.Interiors
{
    public class Area : IInterior
    {
        public List<IInteriorNode> InteriorNodes { get; set; }
        public int InteriorWidth { get; set; }
        public int InteriorHeight { get; set; }
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
    }
}
