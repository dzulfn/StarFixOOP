namespace StarFix.Exceptions
{
    // Custom exception for invalid menu selections
    public class InvalidSelectionException : Exception
    {
        private string _selection;

        public string Selection
        {
            get { return _selection; }
        }

        public InvalidSelectionException()
            : base("Invalid selection was made.")
        {
            _selection = "";
        }

        public InvalidSelectionException(string selection)
            : base("Invalid selection: '" + selection + "'. Please choose a valid option.")
        {
            _selection = selection;
        }

        public InvalidSelectionException(string selection, Exception innerException)
            : base("Invalid selection: '" + selection + "'.", innerException)
        {
            _selection = selection;
        }

        public string GetUserMessage()
        {
            if (string.IsNullOrEmpty(_selection))
                return "You made an invalid selection. Please try again.";

            return "'" + _selection + "' is not a valid option. Please try again.";
        }
    }
}
