using System.Collections.Generic;
using AspiringDemo.Combat;
using AspiringDemo.Factions;
using AspiringDemo.GameActions;
using AspiringDemo.Gamecore;
using AspiringDemo.GameObjects.Squads;
using AspiringDemo.Orders;
using AspiringDemo.Procedural.Interiors;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;

namespace AspiringDemo.GameObjects.Units
{
    public interface IUnit : IGameObject
    {
        ICombatModule CombatModule { get; set; }
        IActionProcesser ActionProcesser { get; set; }
        IItems Items { get; set; }
        List<GameAction> Actions { get; set; }
        IFaction Faction { get; }
        int Hp { get; set; }
        int ID { get; set; }
        bool IsPlayer { get; set; }
        string Name { get; set; }
        IUnitOrder Order { get; set; }
        SquadRank Rank { get; set; }
        UnitState State { get; set; }
        IZone Zone { get; set; }
        RankChanged ChangeRank { get; set; }
        ISquad Squad { get; set; }
        int XPWorth { get; set; }
        IUnitStats Stats { get; set; }
        void AssignOrder(IUnitOrder order);
        void TimeTick(float time);
        void EnterZone(IZone zone);
        void LeaveZone();
        void KilledUnit(IUnit unit);
        void EnterInterior(IInterior interior);
        IInterior Interior { get; set; }
        
    }
}