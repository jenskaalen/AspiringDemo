using System;
using AspiringDemo.Gamecore.Types;

namespace AspiringDemo.Zones.Interiors
{
    [Serializable]
    public class Room : Rect
    {
        public Room(int x1Pos, int y1Pos, int height, int width)
            : base(x1Pos, y1Pos, height, width)
        {
        }
    }
}
