using Communication;
using Bot;
using GameLogic;

var randomBot = new RandomMoveBot();
BotCommunicator botCommunicator = new BotCommunicator(randomBot);
botCommunicator.Start();
