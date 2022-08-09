using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Castlings;

namespace GameLogic.Engine.MoveTypes
{
    public class CastlingMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {
            // Get a new board.
            var nextBoard = board.Copy();

            var castlingConstants = _SelectCastlingConstants(move);

            // Perform castling.
            var kingPiece = nextBoard.GetPiece(castlingConstants.InitialKingCell).Value; // TODO: CS8629
            var rookPiece = nextBoard.GetPiece(castlingConstants.InitialRookCell).Value; // TODO: CS8629

            nextBoard.RemovePiece(castlingConstants.InitialKingCell);
            nextBoard.SetPiece(castlingConstants.FinalKingCell, kingPiece);

            nextBoard.RemovePiece(castlingConstants.InitialRookCell);
            nextBoard.SetPiece(castlingConstants.FinalRookCell, rookPiece);

            return nextBoard;
        }

        private static ICastlingTypeConstants _SelectCastlingConstants(Move move)
        {
            return CastlingConstants.castlingToConstantsMap.Where(pair => pair.Value.CastlingMove == move)
                                                           .Select(pair => pair.Value)
                                                           .First();
        }
    }
}
