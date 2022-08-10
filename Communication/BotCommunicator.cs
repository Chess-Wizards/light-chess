using Bot;

namespace Communication
{
    // BotCommunicator is the entry point for all commands coming to the bot.
    // It is responsible for initial instantiation of the provided protocol and
    // further communication proxying.
    public class BotCommunicator
    {
        private Protocols.IProtocol? _initializedProtocol;
        private readonly IBot _bot;
        private readonly Dictionary<string, Func<IBot, Protocols.IProtocol>>
            _availableCommunicationProtocols = new()
        {
            {"uci", (x) => new Protocols.UCI.UCIProtocol(x)}
        };

        public BotCommunicator(IBot bot)
        {
            _bot = bot;
        }

        private static bool _ShouldQuit(string commandInput)
        {
            return commandInput == "quit";
        }

        public void Start()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (input == null)
                {
                    throw new InvalidOperationException("Invalid input line reading (null).");
                }

                if (_initializedProtocol == null)
                {
                    if (!_availableCommunicationProtocols.ContainsKey(input))
                    {
                        Console.WriteLine("Communication protocol is not initialized. " +
                                           $"Protocol '{input}' is not available. " +
                                           $"List of available protocols: {string.Join(',', _availableCommunicationProtocols.Keys)}");
                        continue;
                    }
                    _initializedProtocol = _availableCommunicationProtocols[input](_bot);
                }

                IEnumerable<string> commandOutput = _initializedProtocol.HandleCommand(input);
                foreach (string output in commandOutput)
                {
                    Console.WriteLine(output);
                }

                if (_ShouldQuit(input))
                {
                    break;
                }
            }
        }
    }
}
