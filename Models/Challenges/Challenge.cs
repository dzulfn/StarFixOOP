namespace SpaceRescueMission.Models.Challenges
{
    // Abstract base class for all challenges (inheritance + polymorphism)
    public abstract class Challenge
    {
        // Private fields
        private string _name;
        private int _difficulty;
        private int _scoreReward;

        // Properties with validation
        public string Name
        {
            get { return _name; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Unknown Challenge";
                else
                    _name = value.Trim();
            }
        }

        public int Difficulty
        {
            get { return _difficulty; }
            protected set
            {
                if (value < 1) _difficulty = 1;
                else if (value > 10) _difficulty = 10;
                else _difficulty = value;
            }
        }

        public int ScoreReward
        {
            get { return _scoreReward; }
            protected set
            {
                if (value > 0) _scoreReward = value;
                else _scoreReward = 10;
            }
        }

        // Constructor
        protected Challenge(string name, int difficulty, int scoreReward = 20)
        {
            Name = name;
            Difficulty = difficulty;
            ScoreReward = scoreReward;
        }

        // Abstract method - each subclass has different behaviour (polymorphism)
        public abstract bool Execute(Player player, Spaceship spaceship);

        // Virtual method - can be overridden
        public virtual string GetDescription()
        {
            return _name + " (Difficulty: " + _difficulty + "/10, Reward: " + _scoreReward + " pts)";
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }
}
