namespace Communication.Protocols.UCI
{
    class ComboOption : BaseOption
    {
        private HashSet<string> allowedValues;
        private string selectedValue;

        public ComboOption(string name, IEnumerable<string> allowedValues, string value) : base(name, "combo")
        {
            this.allowedValues = new HashSet<string>(allowedValues);
            SetValue(value);
        }

        public void SetValue(string value)
        {
            EnsureValueIsAllowed(value);
            this.selectedValue = value;
        }

        private void EnsureValueIsAllowed(string value)
        {
            if (!this.allowedValues.Contains(value))
            {
                throw new UCIProtocolException($"Value '{value}' is not allowed. The list of allowed values for option '{Name}': {String.Join(",", this.allowedValues)}.");
            }
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {this.selectedValue} {String.Join(' ', this.allowedValues.Select(x => "var " + x))}";
        }
    }
}
