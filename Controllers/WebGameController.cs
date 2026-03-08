using SpaceRescueMission.Models;
using SpaceRescueMission.Models.Challenges;
using SpaceRescueMission.Models.Items;
using SpaceRescueMission.Models.Levels;

namespace SpaceRescueMission.Controllers
{
    // Game phases - tracks where the player is in the game
    public enum GamePhase
    {
        MainMenu,
        EnteringName,
        LevelIntro,
        QuizChallenge,
        ChallengeResult,
        LevelComplete,
        LevelFailed,
        BetweenLevels,
        GameWon,
        GameLost
    }

    // Classes for sending data to the browser as JSON

    public class GameStateResponse
    {
        public string Phase { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> Messages { get; set; } = new List<string>();
        public List<OptionInfo> Options { get; set; } = new List<OptionInfo>();
        public bool ShowInput { get; set; }
        public string? InputPlaceholder { get; set; }
        public string? InputAction { get; set; }
        public PlayerInfo? Player { get; set; }
        public ShipInfo? Ship { get; set; }
        public List<ItemInfo>? Items { get; set; }
        public ChallengeData? Challenge { get; set; }
    }

    public class OptionInfo
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Style { get; set; }

        public OptionInfo(string id, string label, string style = "secondary")
        {
            Id = id;
            Label = label;
            Style = style;
        }
    }

    public class PlayerInfo
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public int MaxLives { get; set; }
        public bool IsAlive { get; set; }

        public PlayerInfo(string name, int score, int lives, int maxLives, bool isAlive)
        {
            Name = name;
            Score = score;
            Lives = lives;
            MaxLives = maxLives;
            IsAlive = isAlive;
        }
    }

    public class ShipInfo
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public bool IsOperational { get; set; }

        public ShipInfo(string name, int health, int maxHealth, bool isOperational)
        {
            Name = name;
            Health = health;
            MaxHealth = maxHealth;
            IsOperational = isOperational;
        }
    }

    public class ItemInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUsed { get; set; }

        public ItemInfo(int index, string name, string description, bool isUsed)
        {
            Index = index;
            Name = name;
            Description = description;
            IsUsed = isUsed;
        }
    }

    public class ChallengeData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public int ScoreReward { get; set; }
        public string? QuestionText { get; set; }
        public string[]? QuestionOptions { get; set; }

        public ChallengeData(string type, string name, int difficulty, int scoreReward,
            string? questionText = null, string[]? questionOptions = null)
        {
            Type = type;
            Name = name;
            Difficulty = difficulty;
            ScoreReward = scoreReward;
            QuestionText = questionText;
            QuestionOptions = questionOptions;
        }
    }

    // Main game controller - manages the game using a state machine
    public class WebGameController
    {
        // Game state variables
        private GamePhase _phase = GamePhase.MainMenu;
        private Player _player;
        private Spaceship _spaceship;
        private List<Level> _levels;
        private int _currentLevelIndex;
        private int _currentChallengeIndex;
        private int _challengesPassed;
        private int _itemsAwarded;

        // Retry counter per level
        private int _levelRetryAttempts;
        private const int MaxLevelRetryAttempts = 3;

        // Invalid answer counter per question
        private int _invalidAnswerAttempts;
        private const int MaxInvalidAttempts = 3;

        // For showing messages after each question
        private List<string> _messages = new List<string>();
        private bool _lastChallengeSuccess;

        // Constructor
        public WebGameController()
        {
            _player = new Player("Astronaut");
            _spaceship = new Spaceship("Stellar Voyager");
            _levels = new List<Level>();
            InitializeLevels();
        }

        // Set up the levels
        private void InitializeLevels()
        {
            _levels.Clear();
            _levels.Add(new Level1());
            _levels.Add(new Level2());
            foreach (var level in _levels)
                level.LoadContent();
        }

        // Get current game state (called by API)
        public GameStateResponse GetState()
        {
            return BuildResponse();
        }

        // Process a player action (called by API)
        public GameStateResponse ProcessAction(string action, string value)
        {
            switch (_phase)
            {
                case GamePhase.MainMenu:
                    HandleMainMenu(action);
                    break;
                case GamePhase.EnteringName:
                    HandleEnteringName(action, value);
                    break;
                case GamePhase.LevelIntro:
                    HandleLevelIntro(action);
                    break;
                case GamePhase.QuizChallenge:
                    HandleQuizAction(action, value);
                    break;
                case GamePhase.ChallengeResult:
                    HandleChallengeResult(action, value);
                    break;
                case GamePhase.LevelComplete:
                case GamePhase.LevelFailed:
                    HandleLevelResult(action);
                    break;
                case GamePhase.BetweenLevels:
                    HandleBetweenLevels(action);
                    break;
                case GamePhase.GameWon:
                case GamePhase.GameLost:
                    HandleGameEnd(action);
                    break;
            }

            return BuildResponse();
        }

        // --- Action handlers ---

        private void HandleMainMenu(string action)
        {
            if (action == "start_game")
            {
                _phase = GamePhase.EnteringName;
            }
        }

        private void HandleEnteringName(string action, string value)
        {
            if (action != "submit_name") return;

            if (string.IsNullOrWhiteSpace(value))
                _player.Name = "Astronaut";
            else
                _player.Name = value;

            _player.ResetStats();
            _spaceship = new Spaceship("Stellar Voyager");
            InitializeLevels();

            _currentLevelIndex = 0;
            _currentChallengeIndex = 0;
            _challengesPassed = 0;
            _itemsAwarded = 0;
            _levelRetryAttempts = 0;
            _invalidAnswerAttempts = 0;

            _messages.Clear();
            _messages.Add("Welcome aboard, " + _player.Name + "!");
            _messages.Add("Ship: " + _spaceship.Name);
            _messages.Add("Your mission: rescue the stranded crew in deep space.");
            _messages.Add("You must survive 2 sectors to complete the mission.");
            _messages.Add("");
            _messages.Add("Your ship takes damage from space debris after each question.");
            _messages.Add("Answer correctly to earn items that help you survive!");

            _phase = GamePhase.LevelIntro;
        }

        private void HandleLevelIntro(string action)
        {
            if (action == "continue")
            {
                _currentChallengeIndex = 0;
                _challengesPassed = 0;
                _itemsAwarded = 0;
                _invalidAnswerAttempts = 0;
                _phase = GamePhase.QuizChallenge;
            }
        }

        private void HandleQuizAction(string action, string value)
        {
            if (action != "answer") return;

            var level = _levels[_currentLevelIndex];
            var quiz = level.Challenges[_currentChallengeIndex] as AlienEncounter;
            if (quiz == null) return;

            // Check for invalid (non-numeric or out-of-range) input
            if (!int.TryParse(value, out int answer) || answer < 1 || answer > quiz.Question.Options.Length)
            {
                _invalidAnswerAttempts++;
                _messages.Clear();

                if (_invalidAnswerAttempts >= MaxInvalidAttempts)
                {
                    // Too many invalid attempts - back to main menu
                    _messages.Add("⚠️ Too many invalid inputs. Returning to Main Menu.");
                    ResetToMainMenu();
                    return;
                }

                int left = MaxInvalidAttempts - _invalidAnswerAttempts;
                _messages.Add("⚠️ Invalid answer. Attempts left: " + left);
                // Stay in QuizChallenge phase - don't change _phase
                return;
            }

            // Valid answer - reset invalid counter
            _invalidAnswerAttempts = 0;
            _messages.Clear();

            // Check if answer is correct
            if (quiz.Question.CheckAnswer(answer))
            {
                // Correct answer - light damage (-5)
                _spaceship.TakeDamage(5);
                _player.AddScore(quiz.ScoreReward);
                _messages.Add("☄️ Space debris grazes the ship (-5 hull)");
                _messages.Add("");
                _messages.Add("✅ Correct!");
                _messages.Add("+" + quiz.ScoreReward + " points");
                _lastChallengeSuccess = true;
                _challengesPassed++;
                TryAwardItem();
            }
            else
            {
                // Wrong answer - heavy damage (-30)
                _spaceship.TakeDamage(30);
                _player.LoseLife();
                _messages.Add("☄️ Heavy debris hits the ship! (-30 hull)");
                _messages.Add("");
                _messages.Add("❌ Wrong answer!");
                _messages.Add("Correct answer: " + quiz.Question.GetCorrectAnswerText());
                _messages.Add("Lost 1 life.");
                _lastChallengeSuccess = false;
            }

            _phase = GamePhase.ChallengeResult;
        }

        private void HandleChallengeResult(string action, string value)
        {
            // Handle using items
            if (action == "use_item" && int.TryParse(value, out int idx))
            {
                int i = idx - 1;
                if (i >= 0 && i < _player.Inventory.Count)
                {
                    var item = _player.Inventory[i];
                    if (!item.IsConsumed)
                    {
                        if (item is OxygenTank oxy)
                        {
                            item.Use(_player, _spaceship);
                            _messages.Add("🫁 Used " + item.Name + "! Restored " + oxy.LivesRestored + " life.");
                        }
                        else if (item is RepairKit rep)
                        {
                            if (_spaceship.IsFullyRepaired)
                            {
                                _messages.Add("Ship is already fully repaired! Kit saved.");
                                return;
                            }
                            item.Use(_player, _spaceship);
                            _messages.Add("🔧 Used " + item.Name + "! Repaired " + rep.RepairAmount + " hull points.");
                        }
                        else
                        {
                            item.Use(_player, _spaceship);
                            _messages.Add("Used " + item.Name + ".");
                        }
                    }
                }
                return;
            }

            if (action != "continue") return;

            // Check if player or ship is dead -> GameLost
            if (!_player.IsAlive() || !_spaceship.IsOperational())
            {
                _messages.Clear();
                if (!_player.IsAlive())
                    _messages.Add("You ran out of lives...");
                else
                    _messages.Add("Your ship was destroyed...");
                _messages.Add("Final Score: " + _player.Score);
                _phase = GamePhase.GameLost;
                return;
            }

            // Move to next challenge
            _invalidAnswerAttempts = 0;
            _currentChallengeIndex++;
            var level = _levels[_currentLevelIndex];

            if (_currentChallengeIndex >= level.Challenges.Count)
                CheckAndSetLevelResult();
            else
                _phase = GamePhase.QuizChallenge;
        }

        private void HandleLevelResult(string action)
        {
            if (_phase == GamePhase.LevelFailed)
            {
                if (action == "main_menu")
                {
                    ResetToMainMenu();
                    return;
                }

                if (action != "continue") return;

                _levelRetryAttempts++;

                // Too many retries - send back to main menu
                if (_levelRetryAttempts > MaxLevelRetryAttempts)
                {
                    _messages.Clear();
                    _messages.Add("⚠️ Maximum retry attempts (" + MaxLevelRetryAttempts + ") reached.");
                    _messages.Add("Returned to Main Menu.");
                    ResetToMainMenu();
                    return;
                }

                // Checkpoint - restart the current level
                _player.ResetStats(3);
                _spaceship = new Spaceship("Stellar Voyager");

                Level freshLevel;
                if (_currentLevelIndex == 0)
                    freshLevel = new Level1();
                else
                    freshLevel = new Level2();

                freshLevel.LoadContent();
                _levels[_currentLevelIndex] = freshLevel;

                _messages.Clear();
                _messages.Add("🔄 Retry " + _levelRetryAttempts + "/" + MaxLevelRetryAttempts + " - Restarting level...");
                _messages.Add("");
                _messages.Add("Ship repaired. Lives restored.");
                _messages.Add("🎁 Answer correctly to earn items!");
                _phase = GamePhase.LevelIntro;
                return;
            }

            if (action != "continue") return;

            // Level complete - reset retry counter and move on
            _levelRetryAttempts = 0;
            _currentLevelIndex++;
            if (_currentLevelIndex >= _levels.Count)
                _phase = GamePhase.GameWon;
            else
                _phase = GamePhase.BetweenLevels;
        }

        private void HandleBetweenLevels(string action)
        {
            if (action != "continue") return;

            var level = _levels[_currentLevelIndex];
            _messages.Clear();
            _messages.Add("Entering " + level.Name + "...");
            _messages.Add(level.Description);
            _messages.Add("");
            _messages.Add("Answer correctly to earn items!");

            _currentChallengeIndex = 0;
            _challengesPassed = 0;
            _itemsAwarded = 0;
            _phase = GamePhase.LevelIntro;
        }

        private void HandleGameEnd(string action)
        {
            if (action == "main_menu")
            {
                ResetToMainMenu();
            }
        }

        // Reset everything and go to main menu
        private void ResetToMainMenu()
        {
            _phase = GamePhase.MainMenu;
            _player = new Player("Astronaut");
            _spaceship = new Spaceship("Stellar Voyager");
            _levelRetryAttempts = 0;
            _invalidAnswerAttempts = 0;
            InitializeLevels();
        }

        // --- Helper methods ---

        // Give the player an item as a reward for correct answer
        private void TryAwardItem()
        {
            var level = _levels[_currentLevelIndex];
            if (_itemsAwarded < level.Items.Count)
            {
                var item = level.Items[_itemsAwarded];
                _player.AddItem(item);
                _itemsAwarded++;
                _messages.Add("");
                _messages.Add("🎁 Reward: " + item.Name + " - " + item.Description);
            }
        }

        // Check if player passed enough challenges
        private void CheckAndSetLevelResult()
        {
            var level = _levels[_currentLevelIndex];
            int required = (level.Challenges.Count + 1) / 2;
            bool passed = _player.IsAlive() && _spaceship.IsOperational() && _challengesPassed >= required;

            _messages.Clear();
            _messages.Add("Challenges passed: " + _challengesPassed + "/" + level.Challenges.Count);

            if (passed)
            {
                _messages.Add("All requirements met!");
                _phase = GamePhase.LevelComplete;
            }
            else
            {
                if (!_player.IsAlive())
                    _messages.Add("You ran out of lives...");
                else if (!_spaceship.IsOperational())
                    _messages.Add("Your ship was destroyed...");
                else
                    _messages.Add("Not enough challenges passed.");
                _phase = GamePhase.LevelFailed;
            }
        }

        // --- Build the JSON response for the browser ---

        private GameStateResponse BuildResponse()
        {
            var resp = new GameStateResponse();
            resp.Phase = _phase.ToString();
            resp.Player = BuildPlayerInfo();
            resp.Ship = BuildShipInfo();

            switch (_phase)
            {
                case GamePhase.MainMenu:
                    resp.Title = "SPACE RESCUE MISSION";
                    resp.Messages.Add("A quiz & survival adventure in outer space.");
                    resp.Messages.Add("Answer questions, manage your ship, and rescue the stranded crew!");
                    resp.Options.Add(new OptionInfo("start_game", "Start New Game", "primary"));
                    break;

                case GamePhase.EnteringName:
                    resp.Title = "Enter Your Name";
                    resp.Messages.Add("What shall we call you, astronaut?");
                    resp.ShowInput = true;
                    resp.InputPlaceholder = "Enter your name...";
                    resp.InputAction = "submit_name";
                    break;

                case GamePhase.LevelIntro:
                    var introLevel = _levels[_currentLevelIndex];
                    resp.Title = "Level " + introLevel.LevelNumber + ": " + introLevel.Name;
                    resp.Messages.AddRange(_messages);
                    resp.Messages.Add("");
                    resp.Messages.Add(introLevel.Description);
                    resp.Messages.Add("Questions: " + introLevel.Challenges.Count);
                    resp.Options.Add(new OptionInfo("continue", "Begin", "primary"));
                    break;

                case GamePhase.QuizChallenge:
                    var qLevel = _levels[_currentLevelIndex];
                    var quiz = qLevel.Challenges[_currentChallengeIndex] as AlienEncounter;
                    resp.Title = "Question " + (_currentChallengeIndex + 1) + " of " + qLevel.Challenges.Count;
                    resp.Messages.Add(quiz!.Question.Text);
                    resp.Challenge = new ChallengeData("quiz", quiz.Name, quiz.Difficulty, quiz.ScoreReward,
                        quiz.Question.Text, quiz.Question.Options);
                    for (int i = 0; i < quiz.Question.Options.Length; i++)
                    {
                        resp.Options.Add(new OptionInfo("answer:" + (i + 1), quiz.Question.Options[i], "secondary"));
                    }
                    break;

                case GamePhase.ChallengeResult:
                    if (_lastChallengeSuccess)
                        resp.Title = "Correct!";
                    else
                        resp.Title = "Wrong!";

                    resp.Messages.AddRange(_messages);

                    // Show usable items as buttons
                    if (_player.HasUsableItems())
                    {
                        resp.Messages.Add("");
                        resp.Messages.Add("Use an item?");
                        resp.Items = BuildInventoryList();

                        for (int i = 0; i < resp.Items.Count; i++)
                        {
                            var it = resp.Items[i];
                            if (!it.IsUsed)
                            {
                                string icon = it.Name.Contains("Oxygen") ? "🫁" : "🔧";
                                resp.Options.Add(new OptionInfo("use_item:" + it.Index,
                                    icon + " Use " + it.Name, "secondary"));
                            }
                        }
                    }

                    var curLevel = _levels[_currentLevelIndex];
                    bool moreQuestions = _currentChallengeIndex + 1 < curLevel.Challenges.Count;
                    if (moreQuestions)
                        resp.Options.Add(new OptionInfo("continue", "Next Question", "primary"));
                    else
                        resp.Options.Add(new OptionInfo("continue", "Continue", "primary"));
                    break;

                case GamePhase.LevelComplete:
                    resp.Title = "Level Complete!";
                    resp.Messages.AddRange(_messages);
                    resp.Options.Add(new OptionInfo("continue", "Continue", "primary"));
                    break;

                case GamePhase.LevelFailed:
                    resp.Title = "💔 Level Failed";
                    resp.Messages.AddRange(_messages);
                    int attemptsUsed = _levelRetryAttempts;
                    int attemptsLeft = MaxLevelRetryAttempts - attemptsUsed;
                    resp.Messages.Add("");
                    resp.Messages.Add("Retry attempts used: " + attemptsUsed + "/" + MaxLevelRetryAttempts);
                    if (attemptsLeft > 0)
                    {
                        resp.Messages.Add("Attempts left: " + attemptsLeft);
                        resp.Options.Add(new OptionInfo("continue", "🔄 Retry Level", "primary"));
                    }
                    else
                    {
                        resp.Messages.Add("No retries remaining.");
                    }
                    resp.Options.Add(new OptionInfo("main_menu", "🏠 Main Menu", "secondary"));
                    break;

                case GamePhase.BetweenLevels:
                    var nextLvl = _levels[_currentLevelIndex];
                    resp.Title = "Preparing Next Sector";
                    resp.Messages.Add("Next: Level " + nextLvl.LevelNumber + " - " + nextLvl.Name);
                    resp.Messages.Add(nextLvl.Description);
                    resp.Options.Add(new OptionInfo("continue", "Launch to Next Level", "primary"));
                    break;

                case GamePhase.GameWon:
                    resp.Title = "MISSION COMPLETE!";
                    resp.Messages.Add("You rescued the stranded crew and returned safely!");
                    resp.Messages.Add("Final Score: " + _player.Score);
                    resp.Options.Add(new OptionInfo("main_menu", "Main Menu", "primary"));
                    break;

                case GamePhase.GameLost:
                    resp.Title = "MISSION FAILED";
                    if (!_player.IsAlive())
                        resp.Messages.Add("You ran out of lives in the void of space...");
                    else if (!_spaceship.IsOperational())
                        resp.Messages.Add("Your spaceship was destroyed by space debris...");
                    else
                        resp.Messages.Add("You failed to complete the required challenges.");
                    resp.Messages.Add("Final Score: " + _player.Score);
                    resp.Options.Add(new OptionInfo("main_menu", "Try Again", "primary"));
                    break;
            }

            return resp;
        }

        private PlayerInfo? BuildPlayerInfo()
        {
            if (_phase == GamePhase.MainMenu || _phase == GamePhase.EnteringName)
                return null;
            return new PlayerInfo(_player.Name, _player.Score, _player.Lives, _player.MaxLives, _player.IsAlive());
        }

        private ShipInfo? BuildShipInfo()
        {
            if (_phase == GamePhase.MainMenu || _phase == GamePhase.EnteringName)
                return null;
            return new ShipInfo(_spaceship.Name, _spaceship.Health, _spaceship.MaxHealth, _spaceship.IsOperational());
        }

        private List<ItemInfo> BuildInventoryList()
        {
            var list = new List<ItemInfo>();
            for (int i = 0; i < _player.Inventory.Count; i++)
            {
                var item = _player.Inventory[i];
                list.Add(new ItemInfo(i + 1, item.Name, item.Description, item.IsConsumed));
            }
            return list;
        }
    }
}
