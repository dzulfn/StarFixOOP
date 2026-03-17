namespace StarFix.Models.Challenges
{
    public abstract class Challenge
    {
        private string _name;
        private int _difficulty;
        private int _scoreReward;

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
            protected set { _scoreReward = value > 0 ? value : 10; }
        }

        protected Challenge(string name, int difficulty, int scoreReward = 20)
        {
            Name = name;
            Difficulty = difficulty;
            ScoreReward = scoreReward;
        }

        public abstract bool Execute(Player player, Spaceship spaceship);

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
