namespace Communication
{
    // BotCommunicator is the entry point for all commands coming to the bot.
    // It is responsible for initial instantiation of the provided protocol and
    // and further communication proxying.
    public class BotCommunicator
    {
        private Protocols.IProtocol? initializedProtocol;
        private Dictionary<string, Func<Protocols.IProtocol>> availableCommunicationProtocols =
        new Dictionary<string, Func<Protocols.IProtocol>> {
            {"uci", () => new Protocols.UCI.UCIProtocol()}
            };


        public void Start()
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (input == null)
                {
                    throw new InvalidOperationException("Failed to read input.");
                }

                if (initializedProtocol == null)
                {
                    if (!availableCommunicationProtocols.ContainsKey(input))
                    {
                        Console.WriteLine("Communication protocol is not initialized. " +
                                            $"Protocol '{input}' is not available. " +
                                            "List of available protocols: " + String.Join(",", availableCommunicationProtocols.Keys));
                        continue;
                    }
                    initializedProtocol = availableCommunicationProtocols[input]();
                }

                IEnumerable<string> commandOutput = initializedProtocol.HandleCommand(input);
                foreach (string output in commandOutput)
                {
                    Console.WriteLine(output);
                }
            }
        }
    }
}
