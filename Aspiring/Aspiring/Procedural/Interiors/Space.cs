using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Procedural.Interiors
{
    public class Space : Room
    {
        protected InteriorType Type { get; set; }

        public Space(int x1Pos, int y1Pos, int height, int width) : base(x1Pos, y1Pos, height, width)
        {

        }
    }
}
