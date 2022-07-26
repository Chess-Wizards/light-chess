using GameLogic.Entities.Boards;
using GameLogic.Entities;

namespace GameLogic.Engine.MoveTypes
{
    public class OrdinaryMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();
            var piece = nextBoard.GetPiece(move.StartCell).Value;

            // Perform move.
            nextBoard.SetPiece(move.EndCell, piece);
            nextBoard.RemovePiece(move.StartCell);

            return nextBoard;
        }
    }
}
