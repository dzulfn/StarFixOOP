using StarFix.Interfaces;

namespace StarFix.Models
{
    // Spaceship tracks hull health. Fuel was originally planned but
    // we ended up not using it, so it was removed to keep things simple.
    public class Spaceship : IRepairable, IDisplayable
    {
        private string _name;
        private int _health;
        private int _maxHealth;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Starship";
                else
                    _name = value.Trim();
            }
        }

        public int Health
        {
            get { return _health; }
            private set
            {
                if (value < 0) _health = 0;
                else if (value > _maxHealth) _health = _maxHealth;
                else _health = value;
            }
        }

        public int MaxHealth { get { return _maxHealth; } }

        public bool IsFullyRepaired
        {
            get { return _health >= _maxHealth; }
        }

        public Spaceship(string name, int maxHealth = 100)
        {
            _maxHealth = maxHealth > 0 ? maxHealth : 100;
            Name = name;
            Health = _maxHealth;
        }

        public void Repair(int amount)
        {
            if (amount > 0)
                Health += amount;
        }

        public string GetRepairStatus()
        {
            if (IsFullyRepaired)
                return _name + " is fully repaired.";
            return _name + " health: " + _health + "/" + _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0)
                Health -= damage;
        }

        public bool IsOperational()
        {
            return _health > 0;
        }

        public string GetStatus()
        {
            return "[" + _name + "]  Health: " + _health + "/" + _maxHealth;
        }

        public override string ToString()
        {
            return "Spaceship '" + _name + "' - Health: " + _health + "/" + _maxHealth;
        }
    }
}
