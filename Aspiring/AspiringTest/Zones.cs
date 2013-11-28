using AspiringDemo.GameObjects.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using Ninject;
using AspiringDemo.Factions;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class Zones
    {
        [TestMethod]
        public void Unit_Enter_Zone()
        {
            //IUnit unit = Factories.Instance
            IUnit unit = Factories.Kernel.Get<IUnit>();
            IZone zone = Factories.Kernel.Get<IZone>();

            Assert.IsTrue(zone.Units.Count == 0);

            unit.EnterZone(zone);

            Assert.AreEqual(1, zone.Units.Count);
        }

        [TestMethod]
        public void Units_Enter_Zone_And_Fight_Breaks_Out()
        {
            var faction1 = Factories.Kernel.Get<IFaction>();
            var faction2 = Factories.Kernel.Get<IFaction>();
            var zone1 = Factories.Kernel.Get<IZone>();

            var unit1 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction1));
            var unit2 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction2));

            unit1.EnterZone(zone1);
            unit2.EnterZone(zone1);

            Assert.IsTrue(unit1.CombatModule.CurrentFight != null);
            Assert.IsTrue(unit1.CombatModule.CurrentFight != null);
        }

        [TestMethod]
        public void Dead_Units_Dont_Enter_Fights()
        {
            var faction1 = Factories.Kernel.Get<IFaction>();
            var faction2 = Factories.Kernel.Get<IFaction>();
            var zone1 = Factories.Kernel.Get<IZone>();

            var unit1 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction1));
            var unit2 = Factories.Kernel.Get<IUnit>(new ConstructorArgument("faction", faction2));

            unit1.State = UnitState.Dead;
            unit1.EnterZone(zone1);
            unit2.EnterZone(zone1);

            Assert.IsTrue(unit1.CombatModule.CurrentFight == null);
        }

        [TestMethod]
        public void Get_Closest_Node()
        {
            var zones = Factories.GetZones(10, 10);
            var closest = AspiringDemo.Gamecore.Helpers.Zones.GetClosestZone(zones[10].Position, zones);
            Assert.AreEqual(zones[10], closest);
        }
    }
}
