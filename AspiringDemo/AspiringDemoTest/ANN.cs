using System;
using System.Text;
using System.Collections.Generic;
using AspiringDemo.Factions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo.ANN;
using AspiringDemo.Sites;
using AspiringDemo.Saving;

namespace AspiringDemoTest
{
    /// <summary>
    /// Summary description for ANN
    /// </summary>
    [TestClass]
    public class ANN
    {
        public ANN()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //TODO: Remove this?
        [TestMethod]
        public void GetAndLoadSerializedPopulatedArea()
        {
            IFaction faction = new Faction();
            Outpost outpost = new Outpost(faction, null);
            outpost.Cost = 23;
            outpost.BuildTime = 24;
            outpost.AreaValue = 11;

            string serializedObject = outpost.GetSerializedData();

            Outpost outpost2 = new Outpost(faction, null);
            outpost2.LoadSerializedData(serializedObject);

            Assert.AreEqual(outpost.Cost, outpost2.Cost);
            Assert.AreEqual(outpost.AreaValue, outpost2.AreaValue);
            Assert.AreEqual(outpost.BuildTime, outpost2.BuildTime);
        }

        ////TODO: Remove this?
        //[TestMethod]
        //public void CreateObjectFromFactionSettings()
        //{
        //    IFaction faction = new Faction();
        //    ISerializedTypeData fp = new FactionPreference();
        //    IBuildingManager buildingManager = new BuildingManager();

        //    Outpost outpost = new Outpost(faction, null);
        //    outpost.Cost = 23;
        //    outpost.BuildTime = 24;
        //    outpost.AreaValue = 11;

        //    fp.ObjectType = outpost.GetType();
        //    fp.SerializedData = outpost.GetSerializedData();

        //    buildingManager.BuildingSettings = new List<ISerializedTypeData>();
        //    buildingManager.BuildingSettings.Add(fp);

        //    IPopulatedArea createdArea = buildingManager.CreateAreaDefaultSettings(outpost.GetType());

        //    Assert.AreEqual(outpost.GetType(), createdArea.GetType());
        //}
    }
}
