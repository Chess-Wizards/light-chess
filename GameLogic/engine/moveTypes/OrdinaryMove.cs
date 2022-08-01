using GameLogic.Entities;
using GameLogic.Entities.Boards;

namespace GameLogic.Engine.MoveTypes
{
    public class OrdinaryMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();
            var piece = nextBoard.GetPiece(move.StartCell).Value;

            // Perform a move.
            nextBoard.SetPiece(move.EndCell, piece);
            nextBoard.RemovePiece(move.StartCell);

            return nextBoard;
        }
    }
}
