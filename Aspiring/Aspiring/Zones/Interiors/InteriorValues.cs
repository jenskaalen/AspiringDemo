namespace AspiringDemo.Zones.Interiors
{
    //TODO: consider changing to a struct
    public class InteriorValues
    {
        public int MaxRoomSize { get; private set; }
        public int MinRoomSize { get; private set; }
        public int CorridorWidth { get; private set; }
        public int RoomCount { get; private set; }

        public InteriorValues(int roomCount, int maxRoomSize, int minRoomSize, int corridorWidth)
        {
            MaxRoomSize = maxRoomSize;
            MinRoomSize = minRoomSize;
            CorridorWidth = corridorWidth;
            RoomCount = roomCount;
        }
    }
}
