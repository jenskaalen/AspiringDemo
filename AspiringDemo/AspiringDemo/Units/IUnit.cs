using AspiringDemo.Roleplaying;

namespace AspiringDemo.Units
{
    public interface IUnit
    {
        IWeapon EquippedWeapon { get; set; }
        AspiringDemo.Factions.IFaction Faction { get; }
        int Hp { get; set; }
        int ID { get; set; }
        bool IsPlayer { get; set; }
        int Kills { get; }
        string Name { get; set; }
        AspiringDemo.Orders.IUnitOrder Order { get; set; }
        SquadRank Rank { get; set; }
        int Speed { get; set; }
        UnitState State { get; set; }
        int Toughness { get; set; }
        System.Collections.Generic.List<IWeapon> Weapons { get; set; }
        IZone Zone { get; set; }
        RankChanged ChangeRank { get; set; }
        ISquad Squad { get; set; }

        void AssignOrder(Orders.IUnitOrder order);
        IWeapon SelectBestWeapon();
        int XPWorth { get; set; }
        void TimeTick(long time);

        int GetDamageOutput();

        void KilledUnit(IUnit target);
    }
}
