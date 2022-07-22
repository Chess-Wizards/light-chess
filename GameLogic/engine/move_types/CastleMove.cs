using GameLogic.Entities.Boards;
using GameLogic.Entities;

namespace GameLogic.Engine.MoveTypes
{
    public class CastleMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();

            var deltaX = move.EndCell.X - move.StartCell.X;
            var y = board.GetPiece(move.StartCell)?.Color == Color.White ? 0 : 7;
            int nextXKing;
            int nextXRook;
            int xRook;

            // Short/king castle.
            if (deltaX > 0)
            {
                nextXKing = 6;
                nextXRook = 5;
                xRook = 7;
            }
            // Long/queen castle.
            else
            {
                nextXKing = 2;
                nextXRook = 3;
                xRook = 0;
            }

            var nextKingCell = new Cell(nextXKing, y);
            var nextRookCell = new Cell(nextXRook,
                                        y);
            var rookCell = new Cell(xRook, y);
            // Perform castle.
            var kingPiece = (Piece)nextBoard.GetPiece(move.StartCell);
            var rookPiece = (Piece)nextBoard.GetPiece(rookCell);
            nextBoard.RemovePiece(move.StartCell);
            nextBoard.RemovePiece(rookCell);
            nextBoard.SetPiece(nextKingCell, kingPiece);
            nextBoard.SetPiece(nextRookCell, rookPiece);

            return nextBoard;
        }
    }
}
