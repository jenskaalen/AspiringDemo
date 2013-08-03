using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspiringDemo;
using System.IO;
using System.Linq;
using AspiringDemo.Orders;
using System.Threading;
using System.Diagnostics;

namespace AspiringDemoTest
{
    [TestClass]
    public class OrdersTest
    {

        public OrdersTest()
        {
            if (File.Exists("butterflies.sdf"))
                File.Delete("butterflies.sdf");

            SaveGame savegame = new SaveGame("butterflies");
            Game.SaveGame = savegame;
            Game.ObjectFactory = (IObjectFactory) Game.SaveGame;

            Game.ZonesHeight = 5;
            Game.ZonesWidth = 5;
            Game.Initialize();
            Faction faction1 = new Faction(false, "Raggarbjornanen");
            Faction faction2 = new Faction(false, "Muldvarp-bataljonen");
            Game.AddFaction(faction1);
            Game.AddFaction(faction2);

            Squad muldvarpSquad = faction2.CreateSquad();
            Unit muldvarp1 = Game.ObjectFactory.GetObject<Unit>();
            Unit muldvarp2 = Game.ObjectFactory.GetObject<Unit>();
            Unit muldvarpen = Game.ObjectFactory.GetObject<Unit>();
            muldvarpen.Damage = 50;
            muldvarpen.Name = "Muldvarpen";
            muldvarpSquad.AddMember(muldvarp1);
            muldvarpSquad.AddMember(muldvarp2);
            muldvarpSquad.AddMember(muldvarpen);

            Unit lillebjorn = Game.ObjectFactory.GetObject<Unit>();
            Unit storebjorn = Game.ObjectFactory.GetObject<Unit>();
            Squad bjornesquad = faction1.CreateSquad();
            bjornesquad.AddMember(lillebjorn);
            bjornesquad.AddMember(storebjorn);
        }

        [TestMethod]
        public void TravelOrder()
        {
            Squad tsquad = Game.Factions.First().Squads.First();
            Unit tunit = tsquad.Members.First();

            Debug.WriteLine("woo");

            TravelToZone order = new TravelToZone();
            tunit.Order = order;
            order.Character = tunit;
            order.TargetZone = Game.ZonePathfinder.Nodes[5];
            //tunit.Order.Execute();

            Thread.Sleep(3000);

            Assert.IsTrue(Game.GameTime > 2);
            Assert.AreEqual(CharacterState.ExecutingOrder, tunit.State);
            //tsquad.
        }
    }
}
