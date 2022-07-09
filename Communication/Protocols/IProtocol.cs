namespace Communication.Protocols
{
    // General interface for communication protocols.
    interface IProtocol
    {
        IEnumerable<string> HandleCommand(string input);
    }
}
