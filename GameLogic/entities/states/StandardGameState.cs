using GameLogic.Entities.Boards;

namespace GameLogic.Entities.States
{
    // Represents the game state which has all the required information to continue
    // playing game. 
    //        
    // The game state contains fields/properties inherited from
    // FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation). 
    public class StandardGameState : IStandardGameState
    {
        public IRectangularBoard Board { get; }
        public Color ActiveColor { get; }
        public IEnumerable<Castle> AvailableCastles { get; }
        public Cell? EnPassantCell { get; }
        public int HalfmoveNumber { get; }
        public int FullmoveNumber { get; }
        public Color EnemyColor
        {
            get { return ActiveColor.Change(); }
        }

        public StandardGameState(IRectangularBoard board, Color color, IEnumerable<Castle> availableCastles,
                                 Cell? enPassantCell, int halfmoveNumber, int fullmoveNumber)
        {
            Board = board;
            ActiveColor = color;
            AvailableCastles = availableCastles;
            EnPassantCell = enPassantCell;
            HalfmoveNumber = halfmoveNumber;
            FullmoveNumber = fullmoveNumber;
        }
    }
}
