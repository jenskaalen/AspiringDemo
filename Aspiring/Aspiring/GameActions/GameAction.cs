namespace AspiringDemo.GameActions
{
    public abstract class GameAction
    {
        public bool Finished { get; set; }
        public abstract void Update(float elapsed);
    }
}