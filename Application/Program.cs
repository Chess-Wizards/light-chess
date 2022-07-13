using Communication;
using Bot;
using GameLogic;

BotCommunicator botCommunicator = new BotCommunicator();
botCommunicator.Start();

// Test if random bot works.
var initialFENNotation = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
var randomBot = new RandomMoveBot(initialFENNotation);
var move = (Move)randomBot.SuggestMove();
Console.WriteLine(StandardFENSerializer.MoveToNotation(move));
