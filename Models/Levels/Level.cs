using StarFix.Interfaces;
using StarFix.Models.Challenges;
using StarFix.Models.Items;

namespace StarFix.Models.Levels
{
    // Base level class. Subclasses override LoadContent to define their own
    // questions and items. The actual game loop is in WebGameController —
    // this class just holds the data for each level.
    public class Level : IDisplayable
    {
        private int _levelNumber;
        private string _name;
        private string _description;
        private bool _isCompleted;
        private int _challengesPassed;
        private List<Challenge> _challenges;
        private List<Item> _items;

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
            protected set { _description = value ?? ""; }
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

        public IReadOnlyList<Challenge> Challenges { get { return _challenges.AsReadOnly(); } }
        public IReadOnlyList<Item> Items { get { return _items.AsReadOnly(); } }

        protected List<Challenge> ChallengesList { get { return _challenges; } }
        protected List<Item> ItemsList { get { return _items; } }

        public Level(int levelNumber, string name, string description)
        {
            LevelNumber = levelNumber;
            Name = name;
            Description = description;
            _isCompleted = false;
            _challengesPassed = 0;
            _challenges = new List<Challenge>();
            _items = new List<Item>();
        }

        public virtual void LoadContent()
        {
            // Subclasses add their challenges and items here
        }

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

        public string GetStatus()
        {
            return "[Level " + _levelNumber + ": " + _name + "]  Challenges: " + _challenges.Count;
        }

        public override string ToString()
        {
            return "Level " + _levelNumber + " - " + _name;
        }
    }
}
