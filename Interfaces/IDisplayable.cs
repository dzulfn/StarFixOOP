namespace SpaceRescueMission.Interfaces
{
    // Interface for objects that can display their status
    public interface IDisplayable
    {
        string GetStatus();
        void DisplaySummary();
    }
}
