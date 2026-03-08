using SpaceRescueMission.Interfaces;

namespace SpaceRescueMission.Models
{
    // Represents the player's spaceship
    // Implements IRepairable and IDisplayable
    public class Spaceship : IRepairable, IDisplayable
    {
        // Private fields (encapsulation)
        private string _name;
        private int _health;
        private int _fuelLevel;
        private int _maxHealth;
        private int _maxFuel;

        // Properties with validation
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

        public int FuelLevel
        {
            get { return _fuelLevel; }
            private set
            {
                if (value < 0) _fuelLevel = 0;
                else if (value > _maxFuel) _fuelLevel = _maxFuel;
                else _fuelLevel = value;
            }
        }

        public int MaxHealth { get { return _maxHealth; } }
        public int MaxFuel { get { return _maxFuel; } }

        // IRepairable - check if fully repaired
        public bool IsFullyRepaired
        {
            get { return _health >= _maxHealth; }
        }

        // Constructor
        public Spaceship(string name, int maxHealth = 100, int maxFuel = 100)
        {
            _maxHealth = maxHealth > 0 ? maxHealth : 100;
            _maxFuel = maxFuel > 0 ? maxFuel : 100;
            Name = name;
            Health = _maxHealth;
            FuelLevel = _maxFuel;
        }

        // IRepairable - repair the ship
        public void Repair(int amount)
        {
            if (amount > 0)
                Health += amount;
        }

        // IRepairable - get repair status text
        public string GetRepairStatus()
        {
            if (IsFullyRepaired)
                return _name + " is fully repaired.";

            return _name + " health: " + _health + "/" + _maxHealth;
        }

        // Take damage
        public void TakeDamage(int damage)
        {
            if (damage > 0)
                Health -= damage;
        }

        // Use fuel - returns true if enough fuel
        public bool UseFuel(int amount)
        {
            if (amount <= 0) return true;

            if (_fuelLevel >= amount)
            {
                FuelLevel -= amount;
                return true;
            }

            FuelLevel = 0;
            return false;
        }

        // Add fuel
        public void Refuel(int amount)
        {
            if (amount > 0)
                FuelLevel += amount;
        }

        // Check if ship is still working
        public bool IsOperational()
        {
            return _health > 0;
        }

        // IDisplayable - get short status
        public string GetStatus()
        {
            return "[" + _name + "]  Health: " + _health + "/" + _maxHealth;
        }

        // IDisplayable - display full summary
        public void DisplaySummary()
        {
            Console.WriteLine("  Ship  : " + _name);
            Console.WriteLine("     Health: " + _health + "/" + _maxHealth);
            Console.WriteLine("     Fuel  : " + _fuelLevel + "/" + _maxFuel);
        }

        public override string ToString()
        {
            return "Spaceship '" + _name + "' - Health: " + _health + "/" + _maxHealth;
        }
    }
}
