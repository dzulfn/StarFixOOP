namespace SpaceRescueMission.Models.Items
{
    // Repair kit - repairs spaceship hull (inherits from Item)
    public class RepairKit : Item
    {
        private int _repairAmount;

        public int RepairAmount
        {
            get { return _repairAmount; }
            private set { _repairAmount = value > 0 ? value : 20; }
        }

        public RepairKit(int repairAmount = 25)
            : base("Repair Kit", "Repairs spaceship hull damage sustained during the mission.")
        {
            RepairAmount = repairAmount;
        }

        // Override - repairs ship when used (polymorphism)
        public override void Use(Player player, Spaceship spaceship)
        {
            if (IsConsumed)
            {
                Console.WriteLine("  This repair kit has already been used.");
                return;
            }

            if (spaceship == null)
            {
                Console.WriteLine("  No spaceship available to repair.");
                return;
            }

            if (spaceship.IsFullyRepaired)
            {
                Console.WriteLine("  " + spaceship.Name + " is already fully repaired! Kit saved.");
                return;
            }

            spaceship.Repair(_repairAmount);
            IsConsumed = true;
            Console.WriteLine("  🔧 Used Repair Kit! Repaired " + _repairAmount + " hull points.");
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " (Repairs " + _repairAmount + " hull points)";
        }
    }
}
