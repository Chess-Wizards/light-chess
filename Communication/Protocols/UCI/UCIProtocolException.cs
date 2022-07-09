namespace Communication.Protocols.UCI
{
    public class UCIProtocolException : Exception
    {
        public UCIProtocolException() { }

        public UCIProtocolException(string message) : base(message) { }

        public UCIProtocolException(string message, Exception inner) : base(message, inner) { }
    }
}
