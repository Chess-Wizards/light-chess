namespace Communication.Protocols.UCI
{
    class StringOption : BaseOption
    {
        private const string EmptyValue = "<empty>";
        private string _value;

        public StringOption(string name, string value) : base(name, "string") => SetValue(value);

        public void SetValue(string value)
        {
            if (value == EmptyValue)
            {
                _value = "";
            }

            _value = value;
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {_value}";
        }
    }
}
