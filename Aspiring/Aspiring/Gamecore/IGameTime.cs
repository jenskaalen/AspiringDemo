using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore
{
    public delegate void GameTimeTicker(long time);

    public interface IGameTime
    {
        long Time { get; set; }
        int MilisecondsPerTick { get; set; }
        bool GamePaused { get; set; }
        GameTimeTicker TimeTicker { get; set; }
    }
}
