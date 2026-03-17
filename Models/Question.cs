namespace StarFix.Models
{
    public class Question
    {
        private string _text;
        private string[] _options;
        private int _correctIndex;
        private int _points;

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
            set { _options = value ?? new string[0]; }
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
            set { _points = value > 0 ? value : 10; }
        }

        public Question(string text, string[] options, int correctIndex, int points = 10)
        {
            _options = options ?? new string[0];
            Text = text;
            CorrectIndex = correctIndex;
            Points = points;
        }

        // 1-based index to match the button labels shown in the UI
        public bool CheckAnswer(int answerIndex)
        {
            return (answerIndex - 1) == _correctIndex;
        }

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
