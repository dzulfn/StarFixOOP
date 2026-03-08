namespace SpaceRescueMission.Interfaces
{
    // Interface for objects that can be repaired (e.g. Spaceship)
    public interface IRepairable
    {
        bool IsFullyRepaired { get; }
        void Repair(int amount);
        string GetRepairStatus();
    }
}
