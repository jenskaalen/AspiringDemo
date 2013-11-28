namespace AspiringDemo.Gamecore
{
    public delegate void GameTimeTicker(float time);

    public interface IGameTime
    {
        float Time { get; set; }
        float SecondsPerTick { get; set; }
        bool GamePaused { get; set; }
        GameTimeTicker TimeTicker { get; set; }
    }
}