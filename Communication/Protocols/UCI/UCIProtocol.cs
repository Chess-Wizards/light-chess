using Bot;
using GameLogic;

namespace Communication.Protocols.UCI
{
    public class UCIProtocol : IProtocol
    {
        private int Id {get; set;}
        private IBot Bot {get; set;}
        private StandardGameState GameState {get; set;}
        private const string InitialFENGameState = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        List<IOption> options = new List<IOption>{
            new StringOption("MainDeveloper_TestStringOption", "SherlockKA"),
            new SpinOption("TargetELORating_TestStringOption", 1000, 2000, 1500)
        };

        public UCIProtocol(IBot bot)
        {
            Bot = bot;
        }

        // Generate a random positive integer number 
        private int GeneratePositiveIntegerNumber()
        {
            return Math.Abs(new Random().Next());
        }

        private StandardGameState GetInitialGameState()
        {
            return StandardFENSerializer.DeserializeFromFEN(InitialFENGameState);
        }
        public IEnumerable<string> HandleCommand(string input)
        {
            var splitInput = input.Split(' ');

            if (splitInput[0] == "uci")
            {
                return HandleInitialUCICommand();
            }
            else if (splitInput[0] == "isready")
            {
                return HandleIsReadyCommand();
            }
            else if (splitInput[0] == "go")
            {
                return HandleGoCommand();
            }
            else if (splitInput[0] == "quit")
            {
                return HandleQuitCommand();
            }
            else if (splitInput[0] == "position")
            {
                return HandlePositionCommand(splitInput);
            }

            throw new UCIProtocolException($"Received unknown command '{input}'.");
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Print available options.
        // 2. Print 'uciok'.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandleInitialUCICommand()
        {
            Id = GeneratePositiveIntegerNumber();
            GameState = GetInitialGameState();

            yield return $"id name Lightchess {Id}";
            yield return $"id author the Lightchess developers";

            foreach (var option in options)
            {
                yield return option.ToString();
            }

            yield return "uciok";
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Print readyok.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandleIsReadyCommand()
        {
            yield return "readyok";
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Suggest a move.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandleGoCommand()
        {
            var move = (Move)Bot.SuggestMove(GameState);
            yield return StandardFENSerializer.MoveToNotation(move);
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Suggest a move.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandleQuitCommand()
        {
            yield return "quit";
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Set a position.
        // http://wbec-ridderkerk.nl/html/UCIProtocol.html
        private IEnumerable<string> HandlePositionCommand(string[] splitInput)
        {
            var startposUsed = splitInput[1] == "startpos";

            var firstFENNotationIndex = 2;
            var notationLength = 6;
            GameState = startposUsed ? GetInitialGameState() 
                                     : StandardFENSerializer.DeserializeFromFEN(string.Join(" ", splitInput.Skip(firstFENNotationIndex).Take(notationLength)));

            var firstMoveIndex = startposUsed ? 3 : 9;
            var moves = splitInput.Skip(firstMoveIndex)
                                  .Select(notation => StandardFENSerializer.NotationToMove(notation))
                                  .ToList();
            // Perform moves
            foreach (var move in moves)
            {
                GameState = new StandardGame().MakeMove(GameState, move);
            }

            yield break;
        }
    }
}
