namespace StarFix.Interfaces
{
    public interface IRepairable
    {
        bool IsFullyRepaired { get; }
        void Repair(int amount);
        string GetRepairStatus();
    }
}
