namespace Communication.Protocols.UCI
{
    class ButtonOption : BaseOption
    {
        private readonly Action _action;

        public ButtonOption(string name, Action action) : base(name, "button")
        {
            _action = action;
        }

        protected override string GeneratePostfixForStringRepresentation() => "";
    }
}
