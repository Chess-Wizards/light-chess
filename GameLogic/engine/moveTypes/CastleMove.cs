using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Castles;

namespace GameLogic.Engine.MoveTypes
{
    public class CastleMove : IMoveType<IRectangularBoard>
    {
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {

            // Get a new board.
            var nextBoard = board.Copy();

            var activeColor = board.GetPiece(move.StartCell).Value.Color;
            var castleConstants = _SelectCastleConstants(move);

            // Perform castle.
            var kingPiece = nextBoard.GetPiece(castleConstants.InitialKingCell).Value;
            var rookPiece = nextBoard.GetPiece(castleConstants.InitialRookCell).Value;
            nextBoard.RemovePiece(castleConstants.InitialKingCell);
            nextBoard.RemovePiece(castleConstants.InitialRookCell);
            nextBoard.SetPiece(castleConstants.FinalKingCell, kingPiece);
            nextBoard.SetPiece(castleConstants.FinalRookCell, rookPiece);

            return nextBoard;
        }

        private ICastleTypeConstants _SelectCastleConstants(Move move)
        {
            return CastleConstants.mappingCastleToConstant.Where(pair => pair.Value.CastleMove == move)
                                                          .Select(pair => pair.Value)
                                                          .First();

        }
    }
}
