namespace SpaceRescueMission.Models.Items
{
    // Oxygen tank - restores player lives (inherits from Item)
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

        // Override - restores lives when used (polymorphism)
        public override void Use(Player player, Spaceship spaceship)
        {
            if (IsConsumed)
            {
                Console.WriteLine("  This oxygen tank has already been used.");
                return;
            }

            player.RestoreLives(_livesRestored);
            IsConsumed = true;
            Console.WriteLine("  🫁 Used Oxygen Tank! Restored " + _livesRestored + " life.");
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " (Restores " + _livesRestored + " life)";
        }
    }
}
