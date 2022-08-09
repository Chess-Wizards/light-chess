using GameLogic.Entities.Boards;
using GameLogic.Entities.Castlings;

namespace GameLogic.Entities.States
{
    // Represents the game state with all the required information to continue playing.
    // The game state contains fields/properties inherited from the FEN notation:
    // https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation. 
    public class StandardGameState : IStandardGameState
    {
        public IRectangularBoard Board { get; }
        public IEnumerable<Castling> AvailableCastlings { get; }
        public Cell? EnPassantCell { get; }
        public int HalfmoveNumber { get; }
        public int FullmoveNumber { get; }
        public Color ActiveColor { get; }
        public Color EnemyColor
        {
            get { return ActiveColor.Inversed(); }
        }

        public StandardGameState(IRectangularBoard board, Color color,
                                 IEnumerable<Castling> availableCastles,
                                 Cell? enPassantCell, int halfmoveNumber, int fullmoveNumber)
        {
            Board = board;
            ActiveColor = color;
            AvailableCastlings = availableCastles;
            EnPassantCell = enPassantCell;
            HalfmoveNumber = halfmoveNumber;
            FullmoveNumber = fullmoveNumber;
        }
    }
}
