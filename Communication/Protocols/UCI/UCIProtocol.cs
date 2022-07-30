using Bot;
using GameLogic.Entities.States;
using GameLogic.Engine;

namespace Communication.Protocols.UCI
{
    using CommandInput = IReadOnlyList<string>;
    using CommandOutput = IEnumerable<string>;

    public class UCIProtocol : IProtocol
    {
        private string _NextMoveNotation { get; set; }
        private IBot _Bot { get; }
        private IStandardGameState _GameState { get; set; }

        private static readonly PositionCommandConstants _PositionCommandConstants = new();
        private readonly int _Id;
        private readonly IDictionary<string, Func<CommandInput, CommandOutput>> _mappingHandler;
        private IEnumerable<IOption> _options = new List<IOption> {};

        public UCIProtocol(IBot bot)
        {
            _Bot = bot;
            _Id = _GeneratePositiveIntegerNumber();

            _mappingHandler = new Dictionary<string, Func<IReadOnlyList<string>, IEnumerable<string>>>{
                {"ucinewgame", _HandleUCINewGameCommand},
                {"isready", _HandleIsReadyCommand},
                {"go", _HandleGoCommand},
                {"stop", _HandleStopCommand},
                {"quit", _HandleQuitCommand},
                {"position", _HandlePositionCommand}
            };
        }

        // Generate a random positive integer number 
        private int _GeneratePositiveIntegerNumber()
        {
            return Math.Abs(new Random().Next());
        }

        private IStandardGameState _GetInitialGameState()
        {
            return StandardFENSerializer.DeserializeFromFEN(UCIProtocolConstants.InitialFENGameState);
        }

        public IEnumerable<string> HandleCommand(string input)
        {
            var splitInput = input.Split(UCIProtocolConstants.Delimiter);
            // The first word from |input| is a command 
            var command = splitInput.First();
            var commandHandler = _GetCommand(command);

            return commandHandler(splitInput);
        }

        private Func<IReadOnlyList<string>, IEnumerable<string>> _GetCommand(string command)
        {
            Func<IReadOnlyList<string>, IEnumerable<string>> commandHandler;
            if (_mappingHandler.TryGetValue(command, out commandHandler)) {
                return commandHandler;
            }

            throw new UCIProtocolException($"Received unknown command '{command}'.");
        }

        // As part of the initial 'uci' command the bot has to do following:
        // 1. Prints available options.
        // 2. Prints 'uciok'.
        private IEnumerable<string> _HandleInitialUCICommand(IReadOnlyList<string> splitInput)
        {
            _GameState = _GetInitialGameState();

            yield return $"id name Lightchess {_Id}";
            yield return $"id author the Lightchess developers";
            yield return "";

            foreach (var option in _options)
            {
                yield return option.ToString();
            }

            yield return "uciok";
        }

        // As part of the new game 'uci' command the bot has to do following:
        // 1. Creates a new game.
        private IEnumerable<string> _HandleUCINewGameCommand(IReadOnlyList<string> splitInput)
        {
            _GameState = _GetInitialGameState();
            yield break;
        }

        // As part of the ready 'uci' command the bot has to do following:
        // 1. Prints 'readyok'.
        public IEnumerable<string> _HandleIsReadyCommand(IReadOnlyList<string> splitInput)
        {
            yield return "readyok";
        }

        // As part of the go 'uci' command the bot has to do following:
        // 1. Suggests a move.
        private IEnumerable<string> _HandleGoCommand(IReadOnlyList<string> splitInput)
        {
            var move = _Bot.SuggestMove(_GameState).Value;
            _NextMoveNotation = StandardFENSerializer.MoveToNotation(move);
            yield return $"bestmove {_NextMoveNotation}";
        }
        // As part of the stop 'uci' command the bot has to do following:
        // 1. Returns a move.
        private IEnumerable<string> _HandleStopCommand(IReadOnlyList<string> splitInput)
        {
            yield return _NextMoveNotation;
        }

        // As part of the quit 'uci' command the bot has to do following:
        // 1. Returns a 'quit'.
        private IEnumerable<string> _HandleQuitCommand(IReadOnlyList<string> splitInput)
        {
            yield return "quit";
        }

        // As part of the position 'uci' command the bot has to do following:
        // 1. Sets a position.
        private IEnumerable<string> _HandlePositionCommand(IReadOnlyList<string> splitInput)
        {
            var startPositionUsed = splitInput[1] == _PositionCommandConstants.StartPositionIndicator;

            _GameState = startPositionUsed ? _GetInitialGameState()
                                           : StandardFENSerializer.DeserializeFromFEN(string.Join(UCIProtocolConstants.Delimiter,
                                                                                                   splitInput.Skip(_PositionCommandConstants.FirstFENNotationIndex)
                                                                                                             .Take(_PositionCommandConstants.NotationLength)
                                                                                                )
                                                                                    );

            var firstMoveIndex = startPositionUsed
                                ? _PositionCommandConstants.FirstMoveIndexWithStartPositionIndicator
                                : _PositionCommandConstants.FirstMoveIndexWithoutStartPositionIndicator;
            splitInput.Skip(firstMoveIndex)
                      .Select(notation => StandardFENSerializer.NotationToMove(notation))
                      .ToList()
                      // Perform move
                      .ForEach(move => { _GameState = new StandardGame().MakeMove(_GameState, move); });
            yield break;
        }
    }
}
