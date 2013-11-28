using System.Collections.Generic;
using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.GameObjects.Squads
{
    public interface ISquad
    {
        //IFaction Faction { get; set; }
        int ID { get; set; }
        bool IsVisible { get; set; }
        int KillCounter { get; set; }
        IUnit Leader { get; set; }
        List<IUnit> Members { get; set; }
        SquadState State { get; set; }
        void AddMember(IUnit member);
        void CheckZone();
        void IsZoneHostile();
        void RemoveMember(IUnit member);
        void MemberChangedState(IUnit unit, UnitState state);
        void EnterZone(IZone zone);
    }
}