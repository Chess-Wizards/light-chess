namespace Communication.Protocols.UCI
{
    class SpinOption : BaseOption
    {
        private readonly int _minAllowed;
        private readonly int _maxAllowed;
        private int _value;

        public SpinOption(string name, int minAllowed, int maxAllowed, int value) : base(name, "spin")
        {
            if (minAllowed > maxAllowed)
            {
                throw new UCIProtocolException($"Minimum allowed value '{minAllowed}' cannot be bigger than the maximum value '{maxAllowed}'.");
            }

            _minAllowed = minAllowed;
            _maxAllowed = maxAllowed;
            SetValue(value);
        }

        public void SetValue(int value)
        {
            EnsureInAllowedRange(value);
            _value = value;
        }

        private void EnsureInAllowedRange(int value)
        {
            if (value < _minAllowed || value > _maxAllowed)
            {
                throw new UCIProtocolException($"Option value '{value}' must be in range [{_minAllowed}, {_maxAllowed}].");
            }
        }

        protected override string GeneratePostfixForStringRepresentation()
        {
            return $"default {_value} min {_minAllowed} max {_maxAllowed}";
        }
    }
}
