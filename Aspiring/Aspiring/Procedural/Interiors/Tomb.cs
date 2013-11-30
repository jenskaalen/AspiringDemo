using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Procedural.Interiors
{
    public class Tomb : IInterior
    {
        public List<IInteriorNode> InteriorNodes { get; set; }
        public List<Room> Rooms { get; set; }
        public List<CorridorPath> Paths { get; set; }
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
        }


        public Tomb(int rooms, int width, int height)
        {
            _maxRooms = rooms;
            InteriorWidth = width;
            InteriorHeight = height;
            Paths = new List<CorridorPath>();
            Rooms = new List<Room>();
            InteriorNodes = new List<IInteriorNode>();

            CreateRooms();
            CreateNodes();
            SetEntrance();
        }

        private void SetEntrance()
        {
            Entrance = Rooms[0];
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
                if (!Paths.SelectMany(path => path.ConnectedRooms).Contains(room))
                {
                    // find closest ?
                    CreateCorridors(room, Rooms.Where(room1 => room1 != room).ToList()[GameFrame.Random.Next(0, Rooms.Count - 1)]);
                }
            }
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
