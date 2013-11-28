namespace AspiringDemo.GameCore
{
    public interface IObjectFactory
    {
        T GetObject<T>() where T : class, new();
    }
}