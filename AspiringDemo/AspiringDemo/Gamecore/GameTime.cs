using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore
{
    public class GameTime : IGameTime
    {
        public long Time { get; set; }
        public int MilisecondsPerTick { get; set; }
        public bool GamePaused { get; set; }
        public GameTimeTicker TimeTicker { get; set; }
    }
}
