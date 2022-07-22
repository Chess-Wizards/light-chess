using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine.MoveTypes
{
    public class PawnPromotionMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            var color = board.GetPiece(move.StartCell).Value.Color;
            var nextBoard = new OrdinaryMove().Apply(board, move);

            // Replace pawn with piece after promotion
            var pieceAfterPromotion = new Piece(color, move.PromotionPieceType.Value);
            nextBoard.SetPiece(move.EndCell, pieceAfterPromotion);

            return nextBoard;
        }
    }
}
