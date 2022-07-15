using Communication;
using Bot;
using GameLogic;

// Test if random bot works.
var initialFENNotation = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
var gameState = StandardFENSerializer.DeserializeFromFEN(initialFENNotation);
var randomBot = new RandomMoveBot();
var move = (Move)randomBot.SuggestMove(gameState);
Console.WriteLine(StandardFENSerializer.MoveToNotation(move));

// BotCommunicator botCommunicator = new BotCommunicator();
// botCommunicator.Start();
