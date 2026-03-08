namespace SpaceRescueMission.Models.Items
{
    // Abstract base class for all items (inheritance + polymorphism)
    public abstract class Item
    {
        // Private fields
        private string _name;
        private string _description;
        private bool _isConsumed;

        // Properties with validation
        public string Name
        {
            get { return _name; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Unknown Item";
                else
                    _name = value.Trim();
            }
        }

        public string Description
        {
            get { return _description; }
            protected set
            {
                if (value == null) _description = "";
                else _description = value;
            }
        }

        public bool IsConsumed
        {
            get { return _isConsumed; }
            protected set { _isConsumed = value; }
        }

        // Constructor
        protected Item(string name, string description)
        {
            Name = name;
            Description = description;
            _isConsumed = false;
        }

        // Abstract method - each item type has different effect (polymorphism)
        public abstract void Use(Player player, Spaceship spaceship);

        // Overload for when no spaceship needed
        public void Use(Player player)
        {
            Use(player, null!);
        }

        // Virtual method
        public virtual string GetInfo()
        {
            string status = _isConsumed ? " [USED]" : "";
            return _name + status + " - " + _description;
        }

        public override string ToString()
        {
            return GetInfo();
        }
    }
}
