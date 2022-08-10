namespace Communication.Protocols.UCI
{
    class CheckOption : BaseOption
    {
        private readonly bool _value;

        public CheckOption(string name, bool value = false) : base(name, "check")
        {
            _value = value;
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {_value.ToString().ToLower()}";
        }
    }
}
