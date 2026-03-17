using StarFix.Models.Challenges;
using StarFix.Models.Items;

namespace StarFix.Models.Levels
{
    public class Level1 : Level
    {
        public Level1()
            : base(1, "Earth Orbit", "Navigate through orbital debris and answer basic space trivia to begin your rescue mission.")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

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

            ItemsList.Add(new OxygenTank(livesRestored: 1));
            ItemsList.Add(new RepairKit(repairAmount: 20));
        }
    }
}
