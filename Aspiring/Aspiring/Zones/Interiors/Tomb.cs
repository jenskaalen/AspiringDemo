using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AspiringDemo.Gamecore;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Pathfinding;
using AspiringDemo.Procedural;
using AspiringDemo.Sites;

namespace AspiringDemo.Zones.Interiors
{
    [Serializable]
    public class Tomb : IInterior
    {
        public List<IPathfindingNode> Nodes { get; set; }
        public List<Room> Rooms { get; set; }
        public List<CorridorPath> Paths { get; set; }
        public IEnumerable<IPathfindingNode> Neighbours { get; set; }
        public IPathfindingNode Parent { get; set; }
        public float GValue { get; set; }
        public float HValue { get; set; }
        public float FValue { get; set; }
        public NodeState State { get; set; }
        public List<IZoneEntrance> ZoneEntrances { get; set; }

        public List<IUnit> Units { get; private set; }
        public Room Entrance { get; set; }
        public int InteriorWidth { get; set; }
        public int InteriorHeight { get; set; }
        public bool IsPlayerNearby { get; set; }
        public List<IPopulatedArea> PopulatedAreas { get; set; }
        public Rect Area { get; set; }
        public ZoneType Type { get; set; }
        public Pathfinder<IPathfindingNode> Pathfinder { get; set; }
        private int _maxRoomSize = 15;
        private int _minRoomSize = 15;
        private int _corridorWidth = 5;
        private int _maxRooms = 5;
        private const int HeightBetweenNodes = 1;
        private const int WidthBetweenNodes = 1;

        public void AddEntrance(IZone entrance, Vector2 positionVector2)
        {
            ZoneEntrances.Add(new ZoneEntrance(positionVector2, entrance));
        }

        public Vector2 Position
        {
            get { return Area.Center; }
            set { throw new NotImplementedException(); }
        }

        List<IUnit> IZone.Units
        {
            get { return Units; }
            set { Units = value; }
        }

        public void AddArea(IPopulatedArea area)
        {
            throw new System.NotImplementedException();
        }

        public void AddNeighbour(IZone zone)
        {
            throw new System.NotImplementedException();
        }
        public void Populate(ICreatureGenerator generator)
        {

        }

        public void Enter(IUnit unit)
        {
            unit.Position = Entrance.Center;
            unit.Zone = this;
            Units.Add(unit);
        }

        public Tomb(int xPosition, int yPosition, int width, int height, InteriorValues values)
        {
            ZoneEntrances = new List<IZoneEntrance>();
            _maxRooms = values.RoomCount;
            _maxRoomSize = values.MaxRoomSize;
            _minRoomSize = values.MinRoomSize;
            _corridorWidth = values.CorridorWidth;
            InteriorWidth = width;
            InteriorHeight = height;
            Paths = new List<CorridorPath>();
            Rooms = new List<Room>();
            Nodes = new List<IPathfindingNode>();
            Units = new List<IUnit>();
            Area = new Rect(xPosition, yPosition, height, width);

            CreateRooms();
            CreateNodes();
            SetEntrance();
            Pathfinder = new Pathfinder<IPathfindingNode>();
            Pathfinder.Nodes = Nodes;
            SetNeighbours(width, height);
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
            Nodes = new List<IPathfindingNode>();
            Units = new List<IUnit>();
            Area = new Rect(0, 0, height, width);
            ZoneEntrances = new List<IZoneEntrance>();
            Pathfinder = new Pathfinder<IPathfindingNode>();

            CreateRooms();
            CreateNodes();
            SetEntrance();
            Pathfinder.Nodes = Nodes;
            //TODO: Fox, this takes way too long
            SetNeighbours(WidthBetweenNodes, HeightBetweenNodes);
        }

        private void SetNeighbours(int width, int height)
        {
            Pathfinder.SetNeighbours(width, height);
        }

        private void SetEntrance()
        {
            Entrance = Rooms[0];
        }

        public void CreateDebugImage()
        {
            int sizeMultiplier = 3;
            Bitmap img = new Bitmap(InteriorWidth * 3, InteriorHeight * 3);

            AspiringDemo.Gamecore.Images.DrawRect(img, 0, 0, InteriorWidth * 3, InteriorHeight * 3);


            for (int i = 0; i < InteriorWidth; i++)
            {
                for (int j = 0; j < InteriorHeight; j++)
                {
                    img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.Black);
                }
            }

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var corridor in Paths.SelectMany(path => path.Corridors))
            {
                for (int i = corridor.X1; i < corridor.X2; i++)
                {
                    for (int j = corridor.Y1; j < corridor.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var unit in Units)
            {
                img.SetPixel(unit.Position.X * sizeMultiplier, unit.Position.Y * sizeMultiplier, Color.Orange);
            }

            foreach (var node in Nodes)
            {
                foreach (var nbor in node.Neighbours)
                {
                    AspiringDemo.Gamecore.Images.DrawLine(img, sizeMultiplier,node.Position, nbor.Position);
                }
            }

            img.Save("tomb2.png", ImageFormat.Png);
            img.Dispose();
        }


        public void CreateDebugImage(int x, int y)
        {
            int sizeMultiplier = 3;
            Bitmap img = new Bitmap(InteriorWidth * 3, InteriorHeight * 3);

            AspiringDemo.Gamecore.Images.DrawRect(img, 0, 0, InteriorWidth * 3, InteriorHeight * 3);


            for (int i = 0; i < InteriorWidth; i++)
            {
                for (int j = 0; j < InteriorHeight; j++)
                {
                    img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.Black);
                }
            }

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var corridor in Paths.SelectMany(path => path.Corridors))
            {
                for (int i = corridor.X1; i < corridor.X2; i++)
                {
                    for (int j = corridor.Y1; j < corridor.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var unit in Units)
            {
                img.SetPixel(unit.Position.X * sizeMultiplier, unit.Position.Y * sizeMultiplier, Color.Orange);
            }

            foreach (var node in Nodes)
            {
                foreach (var nbor in node.Neighbours)
                {
                    AspiringDemo.Gamecore.Images.DrawLine(img, sizeMultiplier, node.Position, nbor.Position);
                }
            }

            //show closed nodes
            foreach (var pnt in Pathfinder.ClosedList)
            {
                img.SetPixel(pnt.Position.X * sizeMultiplier, pnt.Position.Y * sizeMultiplier, Color.Red);
            }

            img.SetPixel(x, y, Color.CornflowerBlue);


            //var pathfinder = new Pathfinder<IPathfindingNode>();
            //var foundPath = pathfinder.GetPath(unitPath.First(), unitPath.Last());

            //foreach (var pathnode in foundPath)
            //{
            //    img.SetPixel(pathnode.Position.X, pathnode.Position.Y, Color.Aquamarine);
            //}

            img.Save("tomb2.png", ImageFormat.Png);
            img.Dispose();
        }

        public void CreateDebugImage(IEnumerable<IPathfindingNode> unitPath)
        {
            int sizeMultiplier = 3;
            Bitmap img = new Bitmap(InteriorWidth * 3, InteriorHeight * 3);

            AspiringDemo.Gamecore.Images.DrawRect(img, 0, 0, InteriorWidth * 3, InteriorHeight * 3);


            for (int i = 0; i < InteriorWidth; i++)
            {
                for (int j = 0; j < InteriorHeight; j++)
                {
                    img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.Black);
                }
            }

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var corridor in Paths.SelectMany(path => path.Corridors))
            {
                for (int i = corridor.X1; i < corridor.X2; i++)
                {
                    for (int j = corridor.Y1; j < corridor.Y2; j++)
                    {
                        img.SetPixel(i * sizeMultiplier, j * sizeMultiplier, Color.WhiteSmoke);
                    }
                }
            }

            foreach (var unit in Units)
            {
                img.SetPixel(unit.Position.X * sizeMultiplier, unit.Position.Y * sizeMultiplier, Color.Orange);
            }

            foreach (var node in Nodes)
            {
                foreach (var nbor in node.Neighbours)
                {
                    AspiringDemo.Gamecore.Images.DrawLine(img, sizeMultiplier, node.Position, nbor.Position);
                }
            }

            //show closed nodes
            foreach (var pnt in Pathfinder.ClosedList)
            {
                img.SetPixel(pnt.Position.X * sizeMultiplier, pnt.Position.Y * sizeMultiplier, Color.Red);
            }


            var pathfinder = new Pathfinder<IPathfindingNode>();
            var foundPath = pathfinder.GetPath(unitPath.First(), unitPath.Last());

            foreach (var pathnode in foundPath)
            {
                img.SetPixel(pathnode.Position.X * sizeMultiplier, pathnode.Position.Y * sizeMultiplier, Color.Aquamarine);
            }

            img.Save("tomb2.png", ImageFormat.Png);
            img.Dispose();
        }

        private void CreateNodes()
        {
            Nodes = new List<IPathfindingNode>();

            foreach (var room in Rooms)
            {
                for (int i = room.X1; i < room.X2; i++)
                {
                    for (int j = room.Y1; j < room.Y2; j++)
                    {
                        if (!NodeExists(i, j))
                        {
                            var node = new InteriorNode(i, j);
                            Nodes.Add(node);
                        }
                    }
                }
            }

            foreach (var corridor in Paths.SelectMany(path => path.Corridors))
            {
                for (int i = corridor.X1; i < corridor.X2; i++)
                {
                    for (int j = corridor.Y1; j < corridor.Y2; j++)
                    {
                        if (!NodeExists(i, j))
                        {
                            var node = new InteriorNode(i, j);
                            Nodes.Add(node);
                        }
                        else
                        { 
                        }
                    }
                }
            }
        }

        private bool NodeExists(int x, int y)
        {
            return Nodes.Any(node => node.Position.X == x && node.Position.Y == y);
        }

        private void CreateRooms()
        {
            int maxFails = 500;
            int failCount = 0;

            while (Rooms.Count < _maxRooms && failCount < maxFails)
            {
                var randomRoom = GetRandomRoom();
                bool isValid = Rooms.All(room => !room.Contains(randomRoom));

                if (isValid)
                {
                    Rooms.Add(randomRoom);
                }
                else
                    failCount++;
            }

            if (Rooms.Count < 2)
                return;

            // creating corridors between rooms
            foreach (var room in Rooms)
            {
                var closest = GetClosest(room, Rooms.Where(room1 => room1 != room));
                //var dist = Utility.GetDistance(room.Center, closest.Center);

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

            bool startfromCorridor = verticalCorridor.Center.X < toRoom.Center.X;
            horX = startfromCorridor ? verticalCorridor.X1 : toRoom.X2;
            horY = startUpwards ? verticalCorridor.Y1 : verticalCorridor.Y2;
            horHeight = _corridorWidth;
            horWidth = (startfromCorridor ? toRoom.X1 - verticalCorridor.X2 : verticalCorridor.X1 - toRoom.X2) + _corridorWidth;

            var horizontalCorridor = new Corridor(horX, horY, horHeight, horWidth);
            path.Corridors.Add(horizontalCorridor);

            path.ConnectedRooms.Add(fromRoom);
            path.ConnectedRooms.Add(toRoom);
            Paths.Add(path);
        }

        public int CompareTo(IPathfindingNode other)
        {
            throw new System.NotImplementedException();
        }

        public float DistanceToNode(IPathfindingNode targetNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
