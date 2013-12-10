using System;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using AspiringDemo.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using AspiringDemo.Saving;
using AspiringDemoTest;
using Moq;
using AspiringDemo.ANN.Actions;
using AspiringDemo.Sites;
using AspiringDemo.Factions;
using AspiringDemo.ANN;
using System.Linq;
using AspiringDemo.GameCore;
using Ninject;

namespace AspiringDemoIntegrations
{
    /// <summary>
    /// Integration tests with concrete implementations
    /// </summary>
    [TestClass]
    public class FactionBehaviour
    {
        private IFaction _faction, _faction2;
        private TestSave _save;

        [TestInitialize]
        public void LoadSettings()
        {
            GameFrame.SetGame(new Game());
            _save = new TestSave("Testo");
            GameFrame.Game.Savegame = _save;
            GameFrame.Game.ObjectFactory = _save;

            GameFrame.Game.ZonePathfinder = new AspiringDemo.Pathfinding.Pathfinder<IZone>();
            GameFrame.Game.Initialize();
            GameFrame.Game.ZonePathfinder.Nodes = Factories.GetZones(32, 32);

            _faction = LoadFaction();
            _faction2 = LoadFaction();

            var capitalZone = GameFrame.Game.ZonePathfinder.Nodes[20];
            _faction.CapitalZone = capitalZone;

            var capitalZone2 = GameFrame.Game.ZonePathfinder.Nodes[40];
            _faction2.CapitalZone = capitalZone2;
        }

        ////TOOD: Fix because it relies on random factors
        //[TestMethod]
        //public void FactionFights()
        //{
        //    _faction.Wealth = 7000;
        //    _faction2.Wealth = 7000;

        //    for (int i = 0; i < 300; i++)
        //        GameFrame.Game.GametimeTick();

        //    // assert that at least one faction has killed units
        //    Assert.IsTrue(_faction.Army.Units.Where(x => x.Kills > 0).Count() > 0 || _faction2.Army.Units.Where(x => x.Kills > 0).Count() > 0);
        //    // cant be sure that oen unit gets enough kills to gain a level
        //    //Assert.IsTrue(_faction.Army.Units.Where(x => x.State == UnitState.Dead).Any());
        //}

        [TestMethod]
        public void CreatingBuildingsAndUnits()
        {
            _faction.Army.Units.Add(new Unit(_faction));
            _faction.Wealth = 1000;
            _faction.StructurePoints = 0;

            int originalWealth = _faction.Wealth;
            int originalStructurePoints = _faction.StructurePoints;
            int originalPower = _faction.Power;


            _faction.FactionManager.DetermineAction();

            Assert.AreEqual(1, _faction.FactionManager.QueuedActions.Count);
            Assert.AreEqual(new BuildOutpost(_faction).GetType(), _faction.FactionManager.QueuedActions[0].GetType());
            int buildingCost = ((BuildOutpost)(_faction.FactionManager.QueuedActions[0])).AreaType.Cost;

            _faction.FactionManager.ExecuteFirstActionInQueue();

            Assert.IsTrue(_faction.Areas.Count > 0);
            Assert.IsTrue(_faction.Wealth == originalWealth - buildingCost);

            _faction.FactionManager.DetermineAction();

            Assert.AreEqual(1, _faction.FactionManager.QueuedActions.Count);
            Assert.AreEqual(new RecruitUnit().GetType(), _faction.FactionManager.QueuedActions[0].GetType());

            _faction.FactionManager.ExecuteFirstActionInQueue();

            Assert.IsTrue(_faction.Army.Units.Count > 0);
            Assert.IsTrue(_faction.Power > originalPower);
            Assert.IsTrue(_faction.StructurePoints > originalStructurePoints);
        }

        //TODO: Rewrite this one perhaps
        [TestMethod]
        public void ContinousFactionManagement()
        {
            _faction.Wealth = 3000;
            GameFrame.Game.GameTime.TimeTicker += _faction.GameTimeTick;
            
            for(int i=0; i < 50; i++)
                GameFrame.Game.GametimeTick();

            Assert.IsTrue(_faction.Army.Units.Count > 0);
            Assert.IsTrue(_faction.Areas.Count > 0);
            Assert.IsTrue(_faction.Wealth >= 0);
            Assert.IsTrue(_faction.Wealth < 200);
            // not valid, units will always be given new tasks
            //Assert.AreEqual(3, _faction.Units.Where(x => x.State == UnitState.Idle).Count());
        }

        ////TODO: Remove/rework this?
        private IFaction LoadFaction()
        {
            IFaction faction = NinFactory.Instance.Get<IFaction>();
            faction.Initialize();
            faction.Wealth = 1000;

            //faction.FactionManager = new FactionManager(faction);
            //faction.FactionManager.BuildManager = new BuildingManager(faction);
            //faction.FactionManager.RecruitmentManager = new RecruitmentManager(faction);

            Outpost outpost = new Outpost(faction, null) { AreaValue = 700, BuildTime = 20, Cost = 700 };

            //var fp = new FactionPreference();
            //fp.ObjectType = typeof(Outpost);
            //fp.SerializedData = outpost.GetSerializedData();

            //var settings = new System.Collections.Generic.List<ISerializedTypeData>();
            //settings.Add(fp);

            //faction.FactionManager.BuildManager.BuildingSettings = settings;

            return faction;
        }
    }
}
