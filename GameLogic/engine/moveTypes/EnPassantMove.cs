using GameLogic.Entities;
using GameLogic.Entities.Boards;

namespace GameLogic.Engine.MoveTypes
{
    public class EnPassantMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();
            var piece = board.GetPiece(move.StartCell).Value; // TODO: CS8629

            // Move own pawn.
            nextBoard.RemovePiece(move.StartCell);
            nextBoard.SetPiece(move.EndCell, piece);

            // Capture enemy pawn.
            var enemyCellWithPawn = new Cell(move.EndCell.X, move.StartCell.Y);
            nextBoard.RemovePiece(enemyCellWithPawn); // TODO: check

            return nextBoard;
        }
    }
}
