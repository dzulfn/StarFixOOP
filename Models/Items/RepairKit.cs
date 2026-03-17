namespace StarFix.Models.Items
{
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

        public override void Use(Player player, Spaceship spaceship)
        {
            if (IsConsumed) return;
            if (spaceship.IsFullyRepaired) return;

            spaceship.Repair(_repairAmount);
            IsConsumed = true;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " (Repairs " + _repairAmount + " hull points)";
        }
    }
}
