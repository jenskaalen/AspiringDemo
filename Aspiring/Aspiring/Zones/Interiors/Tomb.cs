using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Procedural;

namespace AspiringDemo.Zones.Interiors
{
    public class Tomb : IInterior
    {
        public List<IInteriorNode> InteriorNodes { get; set; }
        public List<Room> Rooms { get; set; }
        public List<CorridorPath> Paths { get; set; }

        public List<IUnit> Units { get; private set; }

        public Room Entrance { get; set; }
        public int InteriorWidth { get; set; }
        public int InteriorHeight { get; set; }
        private int _maxRoomSize = 15;
        private int _minRoomSize = 15;
        private int _corridorWidth = 5;
        private int _maxRooms = 5;

        public void Populate(ICreatureGenerator generator)
        {

        }

        public void Enter(IUnit unit)
        {
            unit.Position = Entrance.Center;
            unit.Interior = this;
            unit.Zone = null;
            Units.Add(unit);
        }

        public Tomb(int width, int height, InteriorValues values)
        {
            _maxRooms = values.RoomCount;
            _maxRoomSize = values.MaxRoomSize;
            _minRoomSize = values.MinRoomSize;
            _corridorWidth = values.CorridorWidth;
            InteriorWidth = width;
            InteriorHeight = height;
            Paths = new List<CorridorPath>();
            Rooms = new List<Room>();
            InteriorNodes = new List<IInteriorNode>();
            Units = new List<IUnit>();

            CreateRooms();
            CreateNodes();
            SetEntrance();
        }

        private void SetEntrance()
        {
            Entrance = Rooms[0];
        }

        public void CreateDebugImage()
        {
            Bitmap img = new Bitmap(InteriorWidth, InteriorHeight);


            for (int i = 0; i < InteriorWidth; i++)
            {
                for (int j = 0; j < InteriorHeight; j++)
                {
                    img.SetPixel(i, j, Color.Black);
                }
            }

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        img.SetPixel(i, j, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var corridor in Paths.SelectMany(path => path.Corridors))
            {
                for (int i = corridor.X1; i < corridor.X2; i++)
                {
                    for (int j = corridor.Y1; j < corridor.Y2; j++)
                    {
                        img.SetPixel(i, j, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var unit in Units)
            {
                img.SetPixel(unit.Position.X, unit.Position.Y, Color.Orange);
            }

            img.Save("tomb2.png", ImageFormat.Png);
            img.Dispose();
        }

        private void CreateNodes()
        {
            InteriorNodes = new List<IInteriorNode>();

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        var node = new InteriorNode(i, j);
                        InteriorNodes.Add(node);
                    }
                }
            }

        }

        private void CreateRooms()
        {
            while (Rooms.Count < _maxRooms)
            {
                var randomRoom = GetRandomRoom();
                bool isValid = Rooms.All(room => !room.Contains(randomRoom));

                if (isValid)
                {
                    Rooms.Add(randomRoom);
                }
            }

            foreach (var room in Rooms)
            {
                //if (!Paths.SelectMany(path => path.ConnectedRooms).Contains(room))
                //{
                //    // find closest ?
                //    var closest = GetClosest(room, Rooms.Where(room1 => room1 != room));
                //    CreateCorridors(room, Rooms.Where(room1 => room1 != room).ToList()[GameFrame.Random.Next(0, Rooms.Count - 1)]);
                //}
                var closest = GetClosest(room, Rooms.Where(room1 => room1 != room));
                var dist = Utility.GetDistance(room.Center, closest.Center);

                CreateCorridors(room, Rooms.Where(room1 => room1 != room).ToList()[GameFrame.Random.Next(0, Rooms.Count - 1)]);
            }
        }

        private Room GetClosest(Room room, IEnumerable<Room> rooms)
        {
            return rooms.OrderBy(room1 => Utility.GetDistance(room.Center, room1.Center)).First();
        }

        private Room GetRandomRoom()
        {
            int roomW = _minRoomSize + GameFrame.Random.Next(0, _maxRoomSize);
            int roomH = _minRoomSize + GameFrame.Random.Next(0, _maxRoomSize);
            int x = GameFrame.Random.Next(0, InteriorWidth - roomW);
            int y = GameFrame.Random.Next(0, InteriorHeight - roomH);

            var room = new Room(x, y, roomH, roomW);
            return room;
        }

        public void CreateCorridors(Room fromRoom, Room toRoom)
        {
            if (!Paths.Any(path => path.ConnectedRooms.Contains(fromRoom) && path.ConnectedRooms.Contains(toRoom)))
            {
                if (fromRoom.Y1 > toRoom.Y1 || fromRoom.Y2 < toRoom.Y2)
                    CreateVerticalCorridor(fromRoom, toRoom);
            }

            //TODO: add horizontal corridors
        }

        private void CreateVerticalCorridor(Room fromRoom, Room toRoom)
        {
            bool startUpwards = fromRoom.Y1 > toRoom.Y2;
            var path = new CorridorPath();

            int vertX, vertY, vertHeight, vertWidth;

            vertX = fromRoom.Center.X;
            vertHeight = (startUpwards ? fromRoom.Y1 - toRoom.Y2 : toRoom.Y1 - fromRoom.Y2) + _corridorWidth;
            vertWidth = _corridorWidth;
            vertY = startUpwards ? (fromRoom.Y1 - vertHeight) : fromRoom.Y2;

            var verticalCorridor = new Corridor(vertX, vertY, vertHeight, vertWidth);
            path.Corridors.Add(verticalCorridor);

            int horX, horY, horHeight, horWidth;

            // then the horizontal one
            bool startfromCorridor = verticalCorridor.Center.X < toRoom.Center.X;
            horX = startfromCorridor ? verticalCorridor.X1 : toRoom.X2;
            horY = startUpwards ? verticalCorridor.Y1 : verticalCorridor.Y2;
            horHeight = _corridorWidth;
            horWidth = (startfromCorridor ? toRoom.X1 - verticalCorridor.X2 : verticalCorridor.X1 - toRoom.X2) + _corridorWidth;

            var horizontalCorridor = new Corridor(horX, horY, horHeight, horWidth);
            path.Corridors.Add(horizontalCorridor);

            path.ConnectedRooms.Add(fromRoom);
            path.ConnectedRooms.Add(toRoom);
            //path.PathType = String.Format("StartUpwards: {0}, StartFromCorridor: {1}", startUpwards, startfromCorridor);
            Paths.Add(path);
        }
    }
}
