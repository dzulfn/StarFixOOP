namespace StarFix.Models.Challenges
{
    public class AlienEncounter : Challenge
    {
        private Question _question;
        private int _maxAttempts;

        public Question Question
        {
            get { return _question; }
            private set
            {
                if (value == null)
                    _question = new Question("Default question?", new string[] { "A", "B" }, 0);
                else
                    _question = value;
            }
        }

        public int MaxAttempts
        {
            get { return _maxAttempts; }
            private set { _maxAttempts = value > 0 ? value : 2; }
        }

        public AlienEncounter(Question question, int difficulty = 4, int scoreReward = 30, int maxAttempts = 2)
            : base("Alien Encounter", difficulty, scoreReward)
        {
            Question = question;
            MaxAttempts = maxAttempts;
        }

        // Execute exists because Challenge.Execute is abstract, but the actual
        // quiz flow is handled by WebGameController (it needs async web responses).
        // This is only here so AlienEncounter isn't abstract itself.
        public override bool Execute(Player player, Spaceship spaceship)
        {
            throw new NotSupportedException("Quiz flow is handled by WebGameController.");
        }

        public override string GetDescription()
        {
            return base.GetDescription() + " - Quiz: \"" + _question.Text + "\"";
        }
    }
}
