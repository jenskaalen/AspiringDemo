﻿using System.Collections.Generic;
using AspiringDemo.Combat;
using AspiringDemo.GameActions;
using AspiringDemo.Roleplaying;
using AspiringDemo.Roleplaying.Stats;

namespace AspiringDemo.Units
{
    public interface IUnit
    {
        ICombatModule CombatModule { get; set; }
        IActionProcesser ActionProcesser { get; set; }
        IItems Items { get; set; }
        List<GameAction> Actions { get; set; }
        Factions.IFaction Faction { get; }
        int Hp { get; set; }
        int ID { get; set; }
        bool IsPlayer { get; set; }
        int Kills { get; }
        string Name { get; set; }
        //TODO: replace with actions in the action processer
        Orders.IUnitOrder Order { get; set; }
        SquadRank Rank { get; set; }
        UnitState State { get; set; }
        IZone Zone { get; set; }
        RankChanged ChangeRank { get; set; }
        ISquad Squad { get; set; }
        // action...
        void AssignOrder(Orders.IUnitOrder order);
        int XPWorth { get; set; }
        void TimeTick(float time);
        IUnitStats Stats { get; set; }
        void EnterZone(IZone zonudes);
        void LeaveZone();
    }
}
