namespace AspiringDemo.Saving
{
    public interface ICustomSerializable
    {
        void LoadSerializedData(string data);
        string GetSerializedData();
    }
}