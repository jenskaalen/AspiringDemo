using AspiringDemo.GameObjects.Units;

namespace AspiringDemo.Orders
{
    public delegate void OrderFinished();

    public delegate int OrderTick();

    public interface IUnitOrder
    {
        IUnit Unit { get; }
        bool IsExecuting { get; set; }
        bool IsDone { get; set; }
        OrderFinished Finish { get; set; }
        string OrderName { get; }
        void Execute();
        void Update(float gameTime);
    }
}