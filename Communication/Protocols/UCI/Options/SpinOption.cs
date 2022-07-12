namespace Communication.Protocols.UCI
{
    class SpinOption : BaseOption
    {
        private int minAllowed;
        private int maxAllowed;
        private int value;

        public SpinOption(string name, int minAllowed, int maxAllowed, int value) : base(name, "spin")
        {
            if (minAllowed > maxAllowed)
            {
                throw new UCIProtocolException($"Minimum allowed value '{minAllowed}' cannot be bigger than the maximum value '{maxAllowed}'.");
            }

            this.minAllowed = minAllowed;
            this.maxAllowed = maxAllowed;
            SetValue(value);
        }

        public void SetValue(int value)
        {
            EnsureInAllowedRange(value);
            this.value = value;
        }

        private void EnsureInAllowedRange(int value)
        {
            if (value < minAllowed || value > maxAllowed)
            {
                throw new UCIProtocolException($"Option value '{value}' must be in range [{this.minAllowed}, {this.maxAllowed}].");
            }
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {this.value} min {this.minAllowed} max {this.maxAllowed}";
        }
    }
}
