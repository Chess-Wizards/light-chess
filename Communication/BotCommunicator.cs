using Bot;
using System.IO;

namespace Communication
{
    // BotCommunicator is the entry point for all commands coming to the bot.
    // It is responsible for initial instantiation of the provided protocol and
    // further communication proxying.
    public class BotCommunicator
    {
        private Protocols.IProtocol? initializedProtocol;
        private IBot bot;
        private Dictionary<string, Func<IBot, Protocols.IProtocol>> availableCommunicationProtocols =
        new Dictionary<string, Func<IBot, Protocols.IProtocol>> {
            {"uci", (x) => new Protocols.UCI.UCIProtocol(x)}
            };

        public BotCommunicator(IBot bot)
        {
            this.bot = bot;
        }

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
                    initializedProtocol = availableCommunicationProtocols[input](bot);
                }



                IEnumerable<string> commandOutput = initializedProtocol.HandleCommand(input);
                bool isTerminated = false;
                foreach (string output in commandOutput)
                {

                    if (output == "quit")
                    {
                        isTerminated = true;
                        break;
                    }

                    Console.WriteLine(output);
                }

                if (isTerminated)
                {
                    break;
                }
            }
        }
    }
}
