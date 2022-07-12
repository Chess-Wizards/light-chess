namespace Communication.Protocols.UCI
{
    class CheckOption : BaseOption
    {
        private bool value;

        public CheckOption(string name, bool value = false) : base(name, "check")
        {
            this.value = value;
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {this.value.ToString().ToLower()}";
        }
    }
}
