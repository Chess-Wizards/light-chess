using Bot;
using Communication;

var randomBot = new RandomMoveBot();
BotCommunicator botCommunicator = new BotCommunicator(randomBot);
botCommunicator.Start();


// maybe to create a config file with some rules defined, e.g. en passant etc ?
// "KingCanBeCaptured": false - for Fog of War
// "EnPassantEnable": true
// "CastlingUnderCheck": false

// I don't completely understand why we do copy a whole board at each move

// there are only PawnCellsUnderThreat and BishopUnderThreatCells ? + rename


