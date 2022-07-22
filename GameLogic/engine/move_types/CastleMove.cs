using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Castles;

namespace GameLogic.Engine.MoveTypes
{
    public class CastleMove : IMoveType<IRectangularBoard>
    {
        private static readonly StandardBoardConstants _StandardBoardConstants = new();
        public IRectangularBoard Apply(IRectangularBoard board, Move move)
        {

            // Get a new board.
            var nextBoard = board.Copy();

            var activeColor = board.GetPiece(move.StartCell).Value.Color;
            var castleConstants = _SelectCastleConstants(activeColor, move);

            // Perform castle.
            var kingPiece = nextBoard.GetPiece(castleConstants.InitialKingCell).Value;
            var rookPiece = nextBoard.GetPiece(castleConstants.InitialRookCell).Value;
            nextBoard.RemovePiece(castleConstants.InitialKingCell);
            nextBoard.RemovePiece(castleConstants.InitialRookCell);
            nextBoard.SetPiece(castleConstants.FinalKingCell, kingPiece);
            nextBoard.SetPiece(castleConstants.FinalRookCell, rookPiece);

            return nextBoard;
        }

        private ICastleConstant _SelectCastleConstants(Color color, Move move)
        {
            var deltaX = move.EndCell.X - move.StartCell.X;
            if (color == Color.White && deltaX > 0)
            {
                return new WhiteKingCastleConstants();
            }
            else if (color == Color.White && deltaX < 0)
            {
                return new WhiteQueenCastleConstants();
            }
            else if (color == Color.Black && deltaX > 0)
            {
                return new BlackKingCastleConstants();
            }
            else if (color == Color.Black && deltaX < 0)
            {
                return new BlackQueenCastleConstants();
            }

            throw new ArgumentException($"Invalid argument combination {color} and {deltaX}");
        }
    }
}
