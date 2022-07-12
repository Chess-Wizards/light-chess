namespace Communication.Protocols.UCI
{
    // Common interface for all types of options possible
    // in the UCI protocol.
    // More details:
    // http://wbec-ridderkerk.nl/html/UCIProtocol.html
    interface IOption
    {
        string Name { get; }
        string Type { get; }
    }

    abstract class BaseOption : IOption
    {
        public string Name { get; }
        public string Type { get; }

        public BaseOption(string name, string type)
        {
            Name = name;
            Type = type;
        }

        // Generates common part for the option description,
        // which is similar for all options.
        protected string CommonPrefix()
        {
            return $"option name {Name} type {Type}";
        }

        public override string ToString()
        {
            return String.Join(" ", new List<string>{
                CommonPrefix(),
                GeneratePostfixForStringRepresentation()
            });
        }

        abstract protected string GeneratePostfixForStringRepresentation();
    }
}
