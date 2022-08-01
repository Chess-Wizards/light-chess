using Bot;
using Communication;

var randomBot = new RandomMoveBot();
BotCommunicator botCommunicator = new BotCommunicator(randomBot);
botCommunicator.Start();
