using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Factions;
using AspiringDemo.GameActions.Movement;
using AspiringDemo.Gamecore;
using AspiringDemo.Gamecore.Types;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Orders;
using AspiringDemo.Procedural;
using AspiringDemo.Zones;
using AspiringDemo.Zones.Interiors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;
using Ninject.Planning.Targets;

namespace AspiringDemoTest.Interior
{
    [TestClass]
    public class Interiors
    {
        private IUnit _unit;
        private IFaction _faction;

        [TestInitialize]
        public void Init()
        {
            _faction = GameFrame.Game.Factory.Get<IFaction>();
            _unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", _faction));
            GameFrame.Game.Initialize();
        }

        [TestMethod]
        public void Unit_Enter_Interior()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));

            Tomb tomb = GetTestTomb();
            unit.EnterZone(tomb);

            Assert.AreEqual(tomb, unit.Zone);
            Assert.AreEqual(tomb.Entrance.Center, unit.Position);
        }

        [TestMethod]
        public void Unit_Move_From_Exterior_To_Interior()
        {
            var vals = new InteriorValues(1, 10, 5, 4);
            IZone remoteExteriorZone = new Zone(100, 100, 100, 100);
            IZone exteriorZoneWithInterior = new Zone(0,0, 100, 100);
            exteriorZoneWithInterior.AddNeighbour(remoteExteriorZone);
            remoteExteriorZone.AddNeighbour(exteriorZoneWithInterior);
            IZone interiorZone = new Tomb(0, 0, 100, 100, vals);
            GameFrame.Game.ZonePathfinder.Nodes = new List<IZone>() { exteriorZoneWithInterior };

            exteriorZoneWithInterior.AddEntrance(interiorZone, new Vector2(50, 50));
            interiorZone.AddEntrance(exteriorZoneWithInterior, new Vector2(50, 50));

            _unit.EnterZone(remoteExteriorZone);
            _unit.Actions.Add(new MoveFromExteriorToInterior(_unit, interiorZone));

            for(int i=0; i < 30; i++)
                _unit.TimeTick(i);

            Assert.AreEqual(interiorZone, _unit.Zone);
        }

        [TestMethod]
        public void Unit_Searches_For_Enemies_In_Interior()
        {
            var enemyFaction = GameFrame.Game.Factory.Get<IFaction>();
            var enemyUnit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", enemyFaction));

            Tomb tomb = GetTestTomb();
            _unit.EnterZone(tomb);
            enemyUnit.EnterZone(tomb);
            enemyUnit.Position = tomb.Nodes[0].Position;

            var farawayNode =
                tomb.Nodes.First(node => Utility.GetDistance(node.Position, enemyUnit.Position) > 40);
            _unit.Position = farawayNode.Position;

            bool detected = _unit.CombatModule.DetectEnemies();
            Assert.IsFalse(detected);

            farawayNode =
                tomb.Nodes.First(node => Utility.GetDistance(node.Position, enemyUnit.Position) < 15);
            _unit.Position = farawayNode.Position;
            _unit.Position = farawayNode.Position;

            detected = _unit.CombatModule.DetectEnemies();
            Assert.IsTrue(detected);
        }

        [TestMethod]
        public void Unit_Moves_From_Room_To_Room_In_Interior()
        {
            Tomb tomb = GetTestTomb();
            _unit.EnterZone(tomb);

            var targetRoom = tomb.Rooms[2];

            var movePos = new MoveToPosition(_unit, targetRoom.Center);
            _unit.Actions.Add(movePos);

            tomb.CreateDebugImage(movePos.TravelPath);

            //TODO: Kan få en mycket høy travelpath... sjekk om dette er en bug eller ikke
            for (int i=0; i < 150; i++)
                _unit.TimeTick(i);

            Assert.AreEqual(0, _unit.Actions.Count);
        }

        private Tomb GetTestTomb()
        {
            Tomb tomb;

            if (!File.Exists("tombdata.bin"))
            {
                return CreateTestTomb();
            }

            using (var stream = File.OpenRead("tombdata.bin"))
            {
                var serializer = new BinaryFormatter();
                tomb = (Tomb)serializer.Deserialize(stream);
            }

            return tomb;
        }

        private Tomb CreateTestTomb()
        {
            var vals = new InteriorValues(10, 20, 10, 4);
            var tomb = new Tomb(256, 256, vals);

            using (var writer = File.OpenWrite("tombdata.bin"))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(writer, tomb);
            }

            return tomb;
        }
    }
}
