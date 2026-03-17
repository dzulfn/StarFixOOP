namespace StarFix.Models.Items
{
    public abstract class Item
    {
        private string _name;
        private string _description;
        private bool _isConsumed;

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
            protected set { _description = value ?? ""; }
        }

        public bool IsConsumed
        {
            get { return _isConsumed; }
            protected set { _isConsumed = value; }
        }

        protected Item(string name, string description)
        {
            Name = name;
            Description = description;
            _isConsumed = false;
        }

        public abstract void Use(Player player, Spaceship spaceship);

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
