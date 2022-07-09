namespace Communication.Protocols.UCI
{
    class StringOption : BaseOption
    {
        private static string EmptyValue = "<empty>";
        private string value;

        public StringOption(string name, string value) : base(name, "string")
        {
            SetValue(value);
        }

        public void SetValue(string value)
        {
            if (value == EmptyValue)
            {
                this.value = "";
            }

            this.value = value;
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {this.value}";
        }
    }
}
