using SpaceRescueMission.Interfaces;
using SpaceRescueMission.Models.Challenges;
using SpaceRescueMission.Models.Items;

namespace SpaceRescueMission.Models.Levels
{
    // Base class for all game levels (inheritance)
    // Contains challenges and items
    public class Level : IDisplayable
    {
        // Private fields
        private int _levelNumber;
        private string _name;
        private string _description;
        private bool _isCompleted;
        private int _challengesPassed;
        private List<Question> _questions;
        private List<Challenge> _challenges;
        private List<Item> _items;

        // Properties with validation
        public int LevelNumber
        {
            get { return _levelNumber; }
            protected set { _levelNumber = value > 0 ? value : 1; }
        }

        public string Name
        {
            get { return _name; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Unknown Level";
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

        public bool IsCompleted
        {
            get { return _isCompleted; }
            protected set { _isCompleted = value; }
        }

        public int ChallengesPassed
        {
            get { return _challengesPassed; }
            protected set { _challengesPassed = value < 0 ? 0 : value; }
        }

        // Read-only collection access
        public IReadOnlyList<Question> Questions { get { return _questions.AsReadOnly(); } }
        public IReadOnlyList<Challenge> Challenges { get { return _challenges.AsReadOnly(); } }
        public IReadOnlyList<Item> Items { get { return _items.AsReadOnly(); } }

        // Protected lists so subclasses can add content
        protected List<Question> QuestionsList { get { return _questions; } }
        protected List<Challenge> ChallengesList { get { return _challenges; } }
        protected List<Item> ItemsList { get { return _items; } }

        // Constructor
        public Level(int levelNumber, string name, string description)
        {
            LevelNumber = levelNumber;
            Name = name;
            Description = description;
            _isCompleted = false;
            _challengesPassed = 0;
            _questions = new List<Question>();
            _challenges = new List<Challenge>();
            _items = new List<Item>();
        }

        // Virtual method - subclasses override to add content
        public virtual void LoadContent()
        {
            // Empty - subclasses add their own challenges/items
        }

        // Virtual method - runs the level
        public virtual void Start(Player player, Spaceship spaceship)
        {
            _challengesPassed = 0;

            Console.WriteLine();
            Console.WriteLine("==========================================");
            Console.WriteLine("  LEVEL " + _levelNumber + ": " + _name);
            Console.WriteLine("  " + _description);
            Console.WriteLine("==========================================");
            Console.WriteLine("  Challenges: " + _challenges.Count + "  |  Items: " + _items.Count);
            Console.WriteLine();

            // Give items to player
            DistributeItems(player);

            // Run each challenge (polymorphism)
            for (int i = 0; i < _challenges.Count; i++)
            {
                if (!player.IsAlive() || !spaceship.IsOperational())
                {
                    Console.WriteLine("  You can no longer continue...");
                    break;
                }

                Console.WriteLine("  -- Challenge " + (i + 1) + " of " + _challenges.Count + " --");
                bool passed = _challenges[i].Execute(player, spaceship);

                if (passed)
                    _challengesPassed++;

                DisplayStats(player, spaceship);

                if (i < _challenges.Count - 1 && player.IsAlive() && spaceship.IsOperational())
                    OfferInventory(player, spaceship);
            }

            CheckCompletion(player, spaceship);

            Console.WriteLine();
            if (_isCompleted)
                Console.WriteLine("  LEVEL " + _levelNumber + " COMPLETE! (" + _challengesPassed + "/" + _challenges.Count + " passed)");
            else
                Console.WriteLine("  LEVEL " + _levelNumber + " FAILED. (" + _challengesPassed + "/" + _challenges.Count + " passed)");
            Console.WriteLine();
        }

        // Check if level is complete
        public virtual bool CheckCompletion(Player player, Spaceship spaceship)
        {
            int required = (_challenges.Count + 1) / 2;
            _isCompleted = player.IsAlive() && spaceship.IsOperational() && _challengesPassed >= required;
            return _isCompleted;
        }

        public string GetSummary()
        {
            string status = _isCompleted ? "COMPLETE" : "INCOMPLETE";
            return "Level " + _levelNumber + ": " + _name + " [" + status + "]";
        }

        // IDisplayable
        public string GetStatus()
        {
            return "[Level " + _levelNumber + ": " + _name + "]  Challenges: " + _challenges.Count;
        }

        public void DisplaySummary()
        {
            Console.WriteLine("  Level " + _levelNumber + ": " + _name);
            Console.WriteLine("     " + _description);
            Console.WriteLine("     Challenges: " + _challenges.Count + "  |  Items: " + _items.Count);
        }

        public override string ToString()
        {
            return "Level " + _levelNumber + " - " + _name;
        }

        // Helper methods
        protected void DisplayStats(Player player, Spaceship spaceship)
        {
            Console.WriteLine();
            Console.WriteLine("  " + player.GetStatus());
            Console.WriteLine("  " + spaceship.GetStatus());
            Console.WriteLine();
        }

        protected void DistributeItems(Player player)
        {
            if (_items.Count == 0) return;

            Console.WriteLine("  You found some items:");
            foreach (var item in _items)
            {
                player.AddItem(item);
                Console.WriteLine("     + " + item.Name + " - " + item.Description);
            }
            Console.WriteLine();
        }

        protected void OfferInventory(Player player, Spaceship spaceship)
        {
            if (!player.HasUsableItems()) return;

            Console.WriteLine("  Would you like to use an item?");
            Console.WriteLine("    1. Use an item");
            Console.WriteLine("    2. Continue");
            Console.Write("  Choice: ");

            string input = Console.ReadLine() ?? "";

            if (input.Trim() == "1")
            {
                player.ShowInventory();
                Console.Write("  Enter item number (or 0 to cancel): ");
                string itemInput = Console.ReadLine() ?? "";

                if (int.TryParse(itemInput, out int itemChoice) && itemChoice > 0)
                {
                    player.UseItem(itemChoice, spaceship);
                    DisplayStats(player, spaceship);
                }
            }
        }
    }
}
