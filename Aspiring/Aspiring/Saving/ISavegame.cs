namespace AspiringDemo.Saving
{
    public interface ISavegame
    {
        void Create(string databaseName);
        void Save();
        void Load();
    }
}