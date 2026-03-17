using StarFix.Models.Challenges;
using StarFix.Models.Items;

namespace StarFix.Models.Levels
{
    public class Level2 : Level
    {
        public Level2()
            : base(2, "Deep Space", "Venture into deep space to locate the stranded crew. Danger increases!")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            ChallengesList.Add(new AlienEncounter(
                new Question("What are the tiny sparkling lights we see in the night sky?",
                    new string[] { "Birds", "Stars", "Planes", "Clouds" }, 1, 15),
                difficulty: 4, scoreReward: 30, maxAttempts: 2));

            ChallengesList.Add(new AlienEncounter(
                new Question("What do we call the group of planets that go around the Sun?",
                    new string[] { "Star group", "Solar system", "Galaxy team", "Space circle" }, 1, 15),
                difficulty: 4, scoreReward: 30, maxAttempts: 2));

            ChallengesList.Add(new AlienEncounter(
                new Question("Which tool helps us look at stars and planets better?",
                    new string[] { "Microscope", "Telescope", "Bin", "Camera" }, 1, 15),
                difficulty: 5, scoreReward: 35, maxAttempts: 2));

            ChallengesList.Add(new AlienEncounter(
                new Question("What is the name of the place where astronauts travel to?",
                    new string[] { "Ocean", "Space", "Forest", "Desert" }, 1, 15),
                difficulty: 5, scoreReward: 35, maxAttempts: 2));

            ChallengesList.Add(new AlienEncounter(
                new Question("Which planet has beautiful rings around it?",
                    new string[] { "Mercury", "Saturn", "Earth", "Mars" }, 1, 20),
                difficulty: 6, scoreReward: 40, maxAttempts: 2));

            ItemsList.Add(new OxygenTank(livesRestored: 1));
            ItemsList.Add(new RepairKit(repairAmount: 20));
            ItemsList.Add(new RepairKit(repairAmount: 20));
        }
    }
}
