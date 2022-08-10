namespace Communication.Protocols.UCI
{
    class ComboOption : BaseOption
    {
        private readonly HashSet<string> _allowedValues;
        private string _selectedValue;

        public ComboOption(string name, IEnumerable<string> allowedValues, string value) : base(name, "combo")
        {
            _allowedValues = new HashSet<string>(allowedValues);
            SetValue(value);
        }

        public void SetValue(string value)
        {
            EnsureValueIsAllowed(value);
            _selectedValue = value;
        }

        private void EnsureValueIsAllowed(string value)
        {
            if (!_allowedValues.Contains(value))
            {
                throw new UCIProtocolException($"Value '{value}' is not allowed. The list of allowed values for option '{Name}': {string.Join(",", _allowedValues)}.");
            }
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {_selectedValue} {string.Join(' ', _allowedValues.Select(x => "var " + x))}";
        }
    }
}
