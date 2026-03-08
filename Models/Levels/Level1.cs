using SpaceRescueMission.Models.Challenges;
using SpaceRescueMission.Models.Items;

namespace SpaceRescueMission.Models.Levels
{
    // Level 1: Earth Orbit - easier questions
    public class Level1 : Level
    {
        public Level1()
            : base(1, "Earth Orbit", "Navigate through orbital debris and answer basic space trivia to begin your rescue mission.")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Add 5 quiz questions
            ChallengesList.Add(new AlienEncounter(
                new Question("What is the star at the center of our solar system?",
                    new string[] { "Moon", "Sun", "Earth", "Mars" }, 1, 10),
                difficulty: 2, scoreReward: 20, maxAttempts: 3));

            ChallengesList.Add(new AlienEncounter(
                new Question("Which planet do we live on?",
                    new string[] { "Venus", "Jupiter", "Earth", "Saturn" }, 2, 10),
                difficulty: 2, scoreReward: 20, maxAttempts: 3));

            ChallengesList.Add(new AlienEncounter(
                new Question("What do we call a big rock that travels around the Sun in space?",
                    new string[] { "Planet", "Asteroid", "Cloud", "Ocean" }, 1, 10),
                difficulty: 3, scoreReward: 20, maxAttempts: 3));

            ChallengesList.Add(new AlienEncounter(
                new Question("What shines in the sky at night and gives us light from the Sun?",
                    new string[] { "Moon", "Mars", "Jupiter", "Mercury" }, 0, 10),
                difficulty: 3, scoreReward: 25, maxAttempts: 3));

            ChallengesList.Add(new AlienEncounter(
                new Question("Which planet is known as the Red Planet?",
                    new string[] { "Mars", "Earth", "Saturn", "Venus" }, 0, 15),
                difficulty: 3, scoreReward: 25, maxAttempts: 3));

            // Reward items
            ItemsList.Add(new OxygenTank(livesRestored: 1));
            ItemsList.Add(new RepairKit(repairAmount: 20));
        }

        public override void Start(Player player, Spaceship spaceship)
        {
            Console.WriteLine();
            Console.WriteLine("  You are orbiting Earth. The rescue signal is coming from deep space...");
            Console.WriteLine();
            base.Start(player, spaceship);
        }
    }
}
