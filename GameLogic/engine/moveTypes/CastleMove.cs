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

            var castleConstants = _SelectCastleConstants(move);

            // Perform castle.
            var kingPiece = nextBoard.GetPiece(castleConstants.InitialKingCell).Value; // TODO: CS8629
            var rookPiece = nextBoard.GetPiece(castleConstants.InitialRookCell).Value; // TODO: CS8629

            nextBoard.RemovePiece(castleConstants.InitialKingCell);
            nextBoard.SetPiece(castleConstants.FinalKingCell, kingPiece);

            nextBoard.RemovePiece(castleConstants.InitialRookCell);
            nextBoard.SetPiece(castleConstants.FinalRookCell, rookPiece);

            return nextBoard;
        }

        private ICastleTypeConstants _SelectCastleConstants(Move move) // static ?
        {
            return CastleConstants.mappingCastleToConstant.Where(pair => pair.Value.CastleMove == move)
                                                          .Select(pair => pair.Value)
                                                          .First();
        }
    }
}
