namespace AspiringDemo.Units
{
    public interface ISquad
    {
        void AddMember(IUnit member);
        void CheckZone();
        //IFaction Faction { get; set; }
        int ID { get; set; }
        bool IsVisible { get; set; }
        void IsZoneHostile();
        int KillCounter { get; set; }
        IUnit Leader { get; set; }
        System.Collections.Generic.List<IUnit> Members { get; set; }
        void RemoveMember(IUnit member);
        SquadState State { get; set; }
        void MemberChangedState(IUnit unit, UnitState state);
    }
}
