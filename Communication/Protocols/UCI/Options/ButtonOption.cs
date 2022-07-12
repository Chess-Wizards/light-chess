namespace Communication.Protocols.UCI
{
    class ButtonOption : BaseOption
    {
        private Action action;

        public ButtonOption(string name, Action action) : base(name, "button")
        {
            this.action = action;
        }

        protected override string GeneratePostfixForStringRepresentation() => "";
    }
}
