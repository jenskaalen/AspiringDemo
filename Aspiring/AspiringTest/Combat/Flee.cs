using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameObjects;
using AspiringDemo.GameObjects.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Parameters;

namespace AspiringDemoTest.Combat
{
    [TestClass]
    public class Flee
    {
        //Fleeing.
        [TestMethod]
        public void Unit_Wants_To_Flee()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();

            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));
            var unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));
            var unit3 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));
            var unit4 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));

            var fight = GameFrame.Game.Factory.Get<INewFight>();
            fight.Enter(unit);
            fight.Enter(unit2);
            fight.Enter(unit3);
            fight.Enter(unit3);
            fight.Enter(unit4);

            bool wantsToFlee = Fleeing.WantsToFlee(unit, fight);
            Assert.IsTrue(wantsToFlee);

            fight = GameFrame.Game.Factory.Get<INewFight>();

            fight = GameFrame.Game.Factory.Get<INewFight>();
            fight.Enter(unit);
            fight.Enter(unit2);

            wantsToFlee = Fleeing.WantsToFlee(unit, fight);
            Assert.IsFalse(wantsToFlee);
        }

        [TestMethod]
        public void Determine_FleeChance()
        {
            var faction = GameFrame.Game.Factory.Get<IFaction>();
            var faction2 = GameFrame.Game.Factory.Get<IFaction>();

            var unit = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction));
            var unit2 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));
            var unit3 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));
            var unit4 = GameFrame.Game.Factory.Get<IUnit>(new ConstructorArgument("faction", faction2));

            var fight = GameFrame.Game.Factory.Get<INewFight>();
            fight.Enter(unit);
            fight.Enter(unit2);
            fight.Enter(unit3);
            fight.Enter(unit3);
            fight.Enter(unit4);

            double fleeChance = Fleeing.FleeChance(unit.Faction, fight);
            Assert.IsTrue(fleeChance > 95 && fleeChance < 100);
        }
    }
}
