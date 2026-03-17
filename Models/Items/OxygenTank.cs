namespace StarFix.Models.Items
{
    public class OxygenTank : Item
    {
        private int _livesRestored;

        public int LivesRestored
        {
            get { return _livesRestored; }
            private set { _livesRestored = value > 0 ? value : 1; }
        }

        public OxygenTank(int livesRestored = 1)
            : base("Oxygen Tank", "Restores lost lives to keep the astronaut alive in space.")
        {
            LivesRestored = livesRestored;
        }

        public override void Use(Player player, Spaceship spaceship)
        {
            if (IsConsumed) return;
            player.RestoreLives(_livesRestored);
            IsConsumed = true;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " (Restores " + _livesRestored + " life)";
        }
    }
}
