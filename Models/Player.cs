using SpaceRescueMission.Interfaces;
using SpaceRescueMission.Models.Items;

namespace SpaceRescueMission.Models
{
    // Represents the player in the game
    // Implements IDisplayable interface
    public class Player : IDisplayable
    {
        // Private fields (encapsulation)
        private string _name;
        private int _score;
        private int _lives;
        private int _maxLives;
        private List<Item> _inventory;

        // Properties with validation
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Astronaut";
                else
                    _name = value.Trim();
            }
        }

        public int Score
        {
            get { return _score; }
            private set
            {
                if (value < 0)
                    _score = 0;
                else
                    _score = value;
            }
        }

        public int Lives
        {
            get { return _lives; }
            private set
            {
                if (value < 0)
                    _lives = 0;
                else if (value > _maxLives)
                    _lives = _maxLives;
                else
                    _lives = value;
            }
        }

        public int MaxLives
        {
            get { return _maxLives; }
        }

        // Read-only access to inventory
        public IReadOnlyList<Item> Inventory
        {
            get { return _inventory.AsReadOnly(); }
        }

        // Constructor
        public Player(string name, int lives = 3, int maxLives = 5)
        {
            _maxLives = maxLives > 0 ? maxLives : 5;
            _inventory = new List<Item>();
            Name = name;
            Score = 0;
            Lives = lives;
        }

        // Add points to the score
        public void AddScore(int points)
        {
            if (points > 0)
                Score += points;
        }

        // Lose one life
        public void LoseLife()
        {
            Lives--;
        }

        // Restore lives
        public void RestoreLives(int amount)
        {
            if (amount > 0)
                Lives += amount;
        }

        // Check if player is alive
        public bool IsAlive()
        {
            return _lives > 0;
        }

        // Reset stats for a new game
        public void ResetStats(int startingLives = 3)
        {
            Score = 0;
            Lives = startingLives;
            _inventory.Clear();
        }

        // Add item to inventory
        public void AddItem(Item item)
        {
            _inventory.Add(item);
        }

        // Check if player has any usable items
        public bool HasUsableItems()
        {
            foreach (var item in _inventory)
            {
                if (!item.IsConsumed)
                    return true;
            }
            return false;
        }

        // Display inventory list
        public void ShowInventory()
        {
            Console.WriteLine();
            Console.WriteLine("  Inventory:");
            if (_inventory.Count == 0)
            {
                Console.WriteLine("     (empty)");
                return;
            }

            for (int i = 0; i < _inventory.Count; i++)
            {
                string usable = _inventory[i].IsConsumed ? " [USED]" : "";
                Console.WriteLine("     " + (i + 1) + ". " + _inventory[i].Name + usable);
            }
        }

        // Use an item by index (1-based)
        public bool UseItem(int index, Spaceship spaceship)
        {
            int i = index - 1;

            if (i < 0 || i >= _inventory.Count)
            {
                Console.WriteLine("  Invalid item number.");
                return false;
            }

            Item item = _inventory[i];

            if (item.IsConsumed)
            {
                Console.WriteLine("  " + item.Name + " has already been used.");
                return false;
            }

            item.Use(this, spaceship);
            return true;
        }

        // IDisplayable - get short status
        public string GetStatus()
        {
            return "[" + _name + "]  Score: " + _score + "  |  Lives: " + _lives + "/" + _maxLives;
        }

        // IDisplayable - display full summary
        public void DisplaySummary()
        {
            Console.WriteLine("  Player: " + _name);
            Console.WriteLine("     Score : " + _score);
            Console.WriteLine("     Lives : " + _lives + "/" + _maxLives);
            Console.WriteLine("     Items : " + _inventory.Count);
        }

        public override string ToString()
        {
            return "Player '" + _name + "' - Score: " + _score + ", Lives: " + _lives + "/" + _maxLives;
        }

        private int CountUsableItems()
        {
            int count = 0;
            foreach (var item in _inventory)
            {
                if (!item.IsConsumed)
                    count++;
            }
            return count;
        }
    }
}
