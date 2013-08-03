using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo
{
    public struct Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
    }
}
