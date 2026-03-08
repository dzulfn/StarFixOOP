namespace SpaceRescueMission.Models.Challenges
{
    // Quiz challenge - player must answer a question (inherits from Challenge)
    public class AlienEncounter : Challenge
    {
        // Private fields
        private Question _question;
        private int _maxAttempts;

        // Properties
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
            private set
            {
                if (value > 0) _maxAttempts = value;
                else _maxAttempts = 2;
            }
        }

        // Constructor
        public AlienEncounter(Question question, int difficulty = 4, int scoreReward = 30, int maxAttempts = 2)
            : base("Alien Encounter", difficulty, scoreReward)
        {
            Question = question;
            MaxAttempts = maxAttempts;
        }

        // Execute the quiz challenge (polymorphism - overrides Challenge.Execute)
        public override bool Execute(Player player, Spaceship spaceship)
        {
            Console.WriteLine();
            Console.WriteLine("  ALIEN ENCOUNTER - Answer the question!");
            Console.WriteLine("  Difficulty: " + Difficulty + "/10");

            _question.DisplayQuestion();

            int invalidAttempts = 0;

            while (true)
            {
                Console.Write("  Your answer (number): ");
                string input = Console.ReadLine() ?? "";

                if (!int.TryParse(input, out int answer) || answer < 1 || answer > _question.Options.Length)
                {
                    invalidAttempts++;
                    if (invalidAttempts >= _maxAttempts)
                    {
                        Console.WriteLine("  Too many invalid inputs!");
                        break;
                    }
                    Console.WriteLine("  Enter a number between 1 and " + _question.Options.Length);
                    continue;
                }

                if (_question.CheckAnswer(answer))
                {
                    player.AddScore(ScoreReward);
                    Console.WriteLine("  Correct! +" + ScoreReward + " pts");
                    return true;
                }
                else
                {
                    break;
                }
            }

            // Wrong answer
            int damage = Difficulty * 5;
            player.LoseLife();
            spaceship.TakeDamage(damage);
            Console.WriteLine("  Wrong! The correct answer was: " + _question.GetCorrectAnswerText());
            Console.WriteLine("     Lost 1 life. Ship took " + damage + " damage.");
            return false;
        }

        public override string GetDescription()
        {
            return base.GetDescription() + " - Quiz: \"" + _question.Text + "\"";
        }
    }
}
