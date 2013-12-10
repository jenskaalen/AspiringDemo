using System;

namespace AspiringDemo.Gamecore
{
    [Serializable]
    public class GameTime : IGameTime
    {
        public float Time { get; set; }
        public float SecondsPerTick { get; set; }
        public bool GamePaused { get; set; }
        public GameTimeTicker TimeTicker { get; set; }
    }
}