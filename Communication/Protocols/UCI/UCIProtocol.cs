namespace Communication.Protocols.UCI
{
    public class UCIProtocol : IProtocol
    {
        List<IOption> options = new List<IOption>{
            new StringOption("MainDeveloper_TestStringOption", "SherlockKA"),
            new SpinOption("TargetELORating_TestStringOption", 1000, 2000, 1500)
        };

        public IEnumerable<string> HandleCommand(string input)
        {
            if (input == "uci")
            {
                return HandleInitialUCICommand();
            }

            throw new UCIProtocolException($"Received unknown command '{input}'.");
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Print available options.
        // 2. Print 'uciok'.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandleInitialUCICommand()
        {
            foreach (var option in options)
            {
                yield return option.ToString();
            }

            yield return "uciok";
        }
    }
}
