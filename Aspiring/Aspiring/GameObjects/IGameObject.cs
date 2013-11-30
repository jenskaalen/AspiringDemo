namespace AspiringDemo.GameObjects
{
    public interface IGameObject
    {
        Vector2 Position { get; set; }
        IZone Zone { get; set; }
    }
}