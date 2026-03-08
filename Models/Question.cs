namespace SpaceRescueMission.Models
{
    // Represents a multiple-choice quiz question
    public class Question
    {
        // Private fields (encapsulation)
        private string _text;
        private string[] _options;
        private int _correctIndex;
        private int _points;

        // Public properties with validation
        public string Text
        {
            get { return _text; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _text = "No question text";
                else
                    _text = value.Trim();
            }
        }

        public string[] Options
        {
            get { return _options; }
            set
            {
                if (value == null)
                    _options = new string[0];
                else
                    _options = value;
            }
        }

        public int CorrectIndex
        {
            get { return _correctIndex; }
            set
            {
                if (value >= 0 && value < _options.Length)
                    _correctIndex = value;
                else
                    _correctIndex = 0;
            }
        }

        public int Points
        {
            get { return _points; }
            set
            {
                if (value > 0)
                    _points = value;
                else
                    _points = 10;
            }
        }

        // Constructor
        public Question(string text, string[] options, int correctIndex, int points = 10)
        {
            _options = options ?? new string[0];
            Text = text;
            CorrectIndex = correctIndex;
            Points = points;
        }

        // Display the question to console
        public void DisplayQuestion()
        {
            Console.WriteLine();
            Console.WriteLine("  Q: " + _text);
            for (int i = 0; i < _options.Length; i++)
            {
                Console.WriteLine("     " + (i + 1) + ". " + _options[i]);
            }
        }

        // Check if the answer is correct (1-based index)
        public bool CheckAnswer(int answerIndex)
        {
            return (answerIndex - 1) == _correctIndex;
        }

        // Get the correct answer text
        public string GetCorrectAnswerText()
        {
            if (_correctIndex >= 0 && _correctIndex < _options.Length)
                return _options[_correctIndex];
            return "Unknown";
        }

        public override string ToString()
        {
            return "Question: " + _text + " (" + _options.Length + " options, " + _points + " pts)";
        }
    }
}
