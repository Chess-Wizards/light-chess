using Communication;
using Bot;

var randomBot = new RandomMoveBot();
BotCommunicator botCommunicator = new BotCommunicator(randomBot);
botCommunicator.Start();
