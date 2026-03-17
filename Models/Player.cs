using StarFix.Interfaces;
using StarFix.Models.Items;

namespace StarFix.Models
{
    public class Player : IDisplayable
    {
        private string _name;
        private int _score;
        private int _lives;
        private int _maxLives;
        private List<Item> _inventory;

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
            private set { _score = value < 0 ? 0 : value; }
        }

        public int Lives
        {
            get { return _lives; }
            private set
            {
                if (value < 0) _lives = 0;
                else if (value > _maxLives) _lives = _maxLives;
                else _lives = value;
            }
        }

        public int MaxLives { get { return _maxLives; } }

        public IReadOnlyList<Item> Inventory
        {
            get { return _inventory.AsReadOnly(); }
        }

        public Player(string name, int lives = 3, int maxLives = 5)
        {
            _maxLives = maxLives > 0 ? maxLives : 5;
            _inventory = new List<Item>();
            Name = name;
            Score = 0;
            Lives = lives;
        }

        public void AddScore(int points)
        {
            if (points > 0)
                Score += points;
        }

        public void LoseLife()
        {
            Lives--;
        }

        public void RestoreLives(int amount)
        {
            if (amount > 0)
                Lives += amount;
        }

        public bool IsAlive()
        {
            return _lives > 0;
        }

        public void ResetStats(int startingLives = 3)
        {
            Score = 0;
            Lives = startingLives;
            _inventory.Clear();
        }

        public void AddItem(Item item)
        {
            _inventory.Add(item);
        }

        public bool HasUsableItems()
        {
            foreach (var item in _inventory)
            {
                if (!item.IsConsumed)
                    return true;
            }
            return false;
        }

        public string GetStatus()
        {
            return "[" + _name + "]  Score: " + _score + "  |  Lives: " + _lives + "/" + _maxLives;
        }

        public override string ToString()
        {
            return "Player '" + _name + "' - Score: " + _score + ", Lives: " + _lives + "/" + _maxLives;
        }
    }
}
