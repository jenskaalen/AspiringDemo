using System;
using AspiringDemo.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.Linq;
using System.Collections.Generic;
using AspiringDemo.Sites;
using AspiringDemo.Saving;
using AspiringDemo.Orders;
using AspiringDemo.ANN.Actions;
using AspiringDemo.ANN;
using Moq;
using AspiringDemo.Factions;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest
{
    [TestClass]
    public class FactionAI
    {
        private IUnit _storebjorn, _lillebjorn;
        Faction _muldvarpFaction, _bjorneFaction, _faction;
        ISquad _bjorneSquad, _muldvarpSquad;
        FactionPlacementDecider _placementDecider;
        Mock<IFaction> _factionMock;

        [TestInitialize]
        public void Initialize()
        {
            TestSave save = new TestSave("Testo");
            GameFrame.Game.Savegame = save;
            GameFrame.Game.ObjectFactory = save;
            GameFrame.Game.ZonesHeight = 9;
            GameFrame.Game.ZonesWidth = 9;
            GameFrame.Game.Initialize();
            GameFrame.Game.ZonePathfinder.Nodes = Factories.GetZones(12, 12);

            _bjorneFaction = GameFrame.Game.CreateFaction();
            _muldvarpFaction = GameFrame.Game.CreateFaction();
            _faction = GameFrame.Game.CreateFaction();

            // moles
            _muldvarpSquad = _muldvarpFaction.CreateSquad();
            IUnit muldvarp1 = _muldvarpFaction.CreateUnit();
            IUnit muldvarp2 = _muldvarpFaction.CreateUnit();
            IUnit muldvarpen = _muldvarpFaction.CreateUnit();

            muldvarpen.Name = "Muldvarpen";
            muldvarpen.Hp = 150;
            _muldvarpSquad.AddMember(muldvarp1);
            muldvarp1.Name = "dooby";
            _muldvarpSquad.AddMember(muldvarp2);
            _muldvarpSquad.AddMember(muldvarpen);

            muldvarpen.Rank = SquadRank.Commander;

            _lillebjorn = _bjorneFaction.CreateUnit();
            _lillebjorn.Name = "Lillebjorn";
            _storebjorn = _bjorneFaction.CreateUnit();
            _storebjorn.Name = "Storebjorn";
            _bjorneSquad = _bjorneFaction.CreateSquad();

            _bjorneSquad.AddMember(_lillebjorn);
            _bjorneSquad.AddMember(_storebjorn);

            _placementDecider = new FactionPlacementDecider();

            _factionMock = new Mock<IFaction>();
        }

        public FactionAI()
        {
        }

        [TestMethod]
        public void FactionSelectActionPriorities()
        {
            // no need for a mock here!
            Mock<IFaction> factionMock = new Mock<IFaction>();
            factionMock.Setup(x => x.Power).Returns(1000);
            factionMock.Setup(x => x.StructurePoints).Returns(1000);
            factionMock.Setup(x => x.Wealth).Returns(1000);

            //manager.RecruitmentManager = new RecruitmentManager(factionMock.Object);
            //manager.BuildManager = new BuildingManager(factionMock.Object);

            factionMock.Setup(faction1 => faction1.FactionManager)
            .Returns(NinFactory.Instance.Get<IFactionManager>(new ConstructorArgument("faction", factionMock.Object)));


            IFaction faction = factionMock.Object;
            var manager = factionMock.Object.FactionManager;

           

            var bo = new BuildOutpost(faction);
            var ru = new RecruitUnit();

            var boVal = bo.GetPriority(faction);
            var ruVal = ru.GetPriority(faction);

            Assert.IsTrue(0.5 < boVal && 1.5 > boVal);
            Assert.AreEqual(1, ruVal);

            factionMock.Setup(x => x.Power).Returns(950);

            IManagementAction action = manager.DetermineAction();

            Assert.AreEqual(typeof(RecruitUnit), action.GetType());

            factionMock.Setup(x => x.Power).Returns(1050);
            action = manager.DetermineAction();

            Assert.AreEqual(typeof(BuildOutpost), action.GetType());
        }

        [TestMethod]
        public void PlaceBuildingBasedOnPreferences()
        {
            _placementDecider = new FactionPlacementDecider();
            _placementDecider.Faction = _bjorneFaction;
            _placementDecider.MaxDistanceFromCapital = 20000;
            _placementDecider.MinDistanceFromCapital = 2000;
            _placementDecider.MaxDistanceFromFactionZone = 5000;
            _placementDecider.MinDistanceFromFactionZone = 2000;
            _placementDecider.PreferredCapitalDistance = 3000;
            _placementDecider.PreferredFactionZoneDistance = 3000;

            IZone farawayZone = new Zone();
            farawayZone.PositionXStart = 50000;
            farawayZone.PositionXEnd = 51000;
            farawayZone.PositionYStart = 50000;
            farawayZone.PositionYEnd = 51000;

            GameFrame.Game.ZonePathfinder.Nodes.Add(farawayZone);

            Mock<ICapital> capitalMock = new Mock<ICapital>();
            capitalMock.Setup(x => x.Owner).Returns(_bjorneFaction);

            Mock<IPopulatedArea> outpost = new Mock<IPopulatedArea>();
            outpost.Setup(x => x.Owner).Returns(_bjorneFaction);

            var bjorneCapitalZone = GameFrame.Game.ZonePathfinder.Nodes[4];
            bjorneCapitalZone.PopulatedAreas = new List<IPopulatedArea>();
            bjorneCapitalZone.PopulatedAreas.Add(capitalMock.Object);

            _bjorneFaction.CapitalZone = bjorneCapitalZone;

            var bestNode = _placementDecider.GetBestZone(GameFrame.Game.ZonePathfinder.Nodes);
            bestNode.PopulatedAreas = new List<IPopulatedArea>();
            bestNode.PopulatedAreas.Add(outpost.Object);

            var secondBestNode = _placementDecider.GetBestZone(GameFrame.Game.ZonePathfinder.Nodes);

            Assert.AreNotEqual(bestNode, secondBestNode);

            _placementDecider.MaxDistanceFromFactionZone = int.MaxValue;
            _placementDecider.MaxDistanceFromCapital = int.MaxValue;
            _placementDecider.MinDistanceFromCapital = 20000;
            _placementDecider.MinDistanceFromFactionZone = 20000;

            var bestFarawayZone = _placementDecider.GetBestZone(GameFrame.Game.ZonePathfinder.Nodes);

            Assert.AreEqual(farawayZone, bestFarawayZone);
        }

        [TestMethod]
        public void FactionHasHasNoWealthAndWontMakeAnyManagementActions()
        {
            _factionMock.Setup(x => x.StructurePoints).Returns(1000);
            _factionMock.Setup(x => x.Power).Returns(1000);

            _factionMock.Setup(faction => faction.FactionManager).Returns(GameFrame.Game.Factory.Get<IFactionManager>());

            IFactionManager manager = _factionMock.Object.FactionManager;
            
            Assert.IsTrue(manager.BuildManager.AllowedActions.Count > 0);
            Assert.IsTrue(manager.RecruitmentManager.AllowedActions.Count > 0);
            Assert.IsTrue(manager.QueuedActions.Count == 0);

            manager.DetermineAction();
            manager.ExecuteFirstActionInQueue();

            Assert.IsTrue(manager.QueuedActions.Count == 0);
        }

        [TestMethod]
        public void Faction_Collect_Taxes()
        {
            double taxPerCitizen = 0.5;

            Mock<IPopulatedArea> areaMock = new Mock<IPopulatedArea>();
            areaMock.SetupAllProperties();
            areaMock.Object.Population = 100;

            ITaxes taxes = new Taxes();
            
            IFaction faction = new Faction();
            faction.AddArea(areaMock.Object);
            faction.Taxes = taxes;

            int goldFromTaxes = faction.Taxes.CollectTaxes(faction.Areas, taxPerCitizen, 10);
            faction.Wealth += goldFromTaxes;

            Assert.AreEqual(50, goldFromTaxes);
            Assert.AreEqual(50, faction.Wealth);
        }

        //[TestMethod]
        //public void DecisionAssessment()
        //{
        //    bjorneFaction.Wealth = 1000;
        //    bjorneFaction.Power = 1000;

        //    bjorneFaction.FactionManager.DetermineAction();

        //    //Assert.AreEqual(typeof(BuildOutpost), bjorneFaction.FactionManager.
        //}

        //[TestMethod]
        //public void DetermineFactionsActions() 
        //{
        //    bjorneFaction.FactionManager.DetermineActions();

        //    List<IPopulatedArea> allAreas = bjorneFaction.Areas;

        //    Assert.AreEqual(0, allAreas.Count);

        //    var order = _storebjorn.Order;

        //    Assert.AreEqual(typeof(Order), typeof(BuildOrder));
        //}
    }
}




