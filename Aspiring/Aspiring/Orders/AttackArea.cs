using AspiringDemo.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspiringDemo.Units;

namespace AspiringDemo.Orders
{
    //TODO: lol, remove

    //public class AttackArea : IUnitOrder
    //{
    //    public IUnit Unit { get; set; }
    //    public bool IsExecuting { get; set; }
    //    public bool IsDone { get; set; }
    //    public IZone TargetZone { get; set; }
    //    public List<IZone> TravelPath { get; set; }
    //    public OrderFinished Finish { get; set; }
    //    public IPopulatedArea AreaToAttack { get; set; }
    //    public string OrderName { get { return this.ToString(); } }

    //    private IZone _startZone = null;
    //    private long _nextWorkTime;

    //    public AttackArea(Squad squad, IPopulatedArea area)
    //    {
    //        Unit = squad.Leader;
    //        AreaToAttack = area;
    //    }

    //    public AttackArea(IUnit unit, IPopulatedArea area)
    //    {
    //        Unit = unit;
    //        AreaToAttack = area;
    //    }

    //    public void Execute()
    //    {
    //        if (Unit == null)
    //            throw new Exception("Cant execute without unit set");

    //        if (TravelPath == null)
    //            throw new Exception("Cant execute without TravelPath set");

    //        Unit.State = UnitState.ExecutingOrder;
    //        _startZone = Unit.Zone;
    //        IsExecuting = true;
    //        IsDone = false;
    //        _nextWorkTime = GameFrame.Game.TimeToTravelThroughZone;

    //        var factionArmy = Unit.Faction.Army;
    //        var squad = factionArmy.GetUnitSquad(Unit);

    //        //// leader give orders
    //        // leader shouldnt give orders here. a leader might decide to attack an area alone without his squad
    //        if (squad != null && Unit == squad.Leader)
    //        {
    //            var members = squad.Members.Where(x => x != Unit);

    //            foreach (var member in members)
    //            {
    //                var followOrder = new FollowOrder(member, Unit);
    //                Unit.AssignOrder(followOrder);
    //                //throw new NotImplementedException();
    //                //member.Faction.RegisterOrder(followOrder, member);
    //            }
    //        }
    //    }

    //    public void Update(float gameTime)
    //    {
    //        if (Unit.Zone == TargetZone)
    //        {
    //            if (AreaToAttack.Razed)
    //            {
    //                OrderAccomplished();
    //                return;
    //            }
    //            else if (AreaToAttack.IsUnderAttack && && TargetZone.Fight == null)
    //            {
    //                //AreaToAttack.Razed = true;
    //                OrderAccomplished();
    //            }
    //        }
    //        else if (Unit.State == UnitState.ExecutingOrder)
    //        {
    //            if (Unit.Zone.IsPlayerNearby)
    //            {
    //                throw new NotImplementedException();
    //            }
    //            else
    //            {
    //                if (_nextWorkTime < gameTime)
    //                {
    //                    Unit.EnterZone(TravelPath.First());
    //                    TravelPath.Remove(Unit.Zone);
    //                    _nextWorkTime += GameFrame.Game.TimeToTravelThroughZone;
    //                }
    //            }
    //        }
    //    }

    //    public void OrderAccomplished()
    //    {
    //        Unit.Order = null;

    //        if (Unit.State == UnitState.ExecutingOrder)
    //            Unit.State = UnitState.Idle;
    //    }
    //}
}
