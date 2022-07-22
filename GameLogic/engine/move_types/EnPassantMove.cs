using GameLogic.Entities.Boards;
using GameLogic.Entities;

namespace GameLogic.Engine.MoveTypes
{
    public class EnPassantMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();
            var piece = (Piece)board.GetPiece(move.StartCell);

            // Move own pawn.
            nextBoard.SetPiece(move.EndCell, piece);
            nextBoard.RemovePiece(move.StartCell);

            // Capture enemy pawn.
            var enemyCellWithPawn = new Cell(move.EndCell.X, move.StartCell.Y);
            nextBoard.RemovePiece(enemyCellWithPawn);

            return nextBoard;
        }
    }
}
