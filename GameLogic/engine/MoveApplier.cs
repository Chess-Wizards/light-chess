using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.States;
using GameLogic.Entities.Castles;
using GameLogic.Entities.Pieces;
using GameLogic.Engine.MoveTypes;

namespace GameLogic.Engine
{
    // Applies a move.
    public static class MoveApplier
    {
        private static readonly PieceConstants _PieceConstants = new();

        private static readonly CastleConstants _CastleConstants = new();

        // Applies move and returns next game state.
        //
        // Parameters
        // ----------
        // gameState: The start/initial game state.
        // move: The move to perform. The move must be valid/possible.
        //
        // Returns
        // -------
        // The next game state.
        public static IStandardGameState ApplyMove(this IStandardGameState gameState, Move move)
        {
            var optionalPiece = gameState.Board.GetPiece(move.StartCell);
            if (optionalPiece == null)
            {
                throw new ArgumentException("The start/initial cell does not contain a piece.");
            }

            var piece = optionalPiece.Value;
            var moveType = _SelectMoveType(gameState, piece, move);
            var nextBoard = moveType.Apply(gameState.Board, move);
            // Next castles.
            var nextAvailableCastles = _GetCastlesAfterMove(gameState.Board, move, gameState.AvailableCastles);

            // Next cells. 
            var nextEnPassantCell = _GetEnPassantCellAfterMove(gameState.Board, move);

            // Next HalfmoveNumber.
            var movePawn = piece.Type == PieceType.Pawn;
            var moveCapture = !gameState.Board.IsEmpty(move.EndCell) ||
                            move.EndCell == gameState.EnPassantCell;
            var nextHalfmoveNumber = movePawn || moveCapture ? 0 : gameState.HalfmoveNumber + 1;

            // Next FullmoveNumber.
            var nextFullmoveNumber = gameState.FullmoveNumber + 1;

            return new StandardGameState(
                nextBoard,
                gameState.EnemyColor,
                nextAvailableCastles,
                nextEnPassantCell,
                nextHalfmoveNumber,
                nextFullmoveNumber
            );
        }

        private static bool _IsPawnPromotionRank(int rank)
        {
            return _PieceConstants.BlackPawnPromotionRank == rank || _PieceConstants.WhitePawnPromotionRank == rank;
        }

        private static IMoveType<IRectangularBoard> _SelectMoveType(IStandardGameState gameState, Piece startCellPiece, Move move)
        {
            if (startCellPiece.Type == PieceType.King
                && _CastleConstants.mappingCastleToConstant.Values.Any(castleConstant => castleConstant.GetCastleMove == move))
            {
                return new CastleMove();
            }
            // En passant move.
            else if (gameState.EnPassantCell != null
                    && move.EndCell == (Cell)gameState.EnPassantCell)
            {
                return new EnPassantMove();
            }
            // Pawn promotion
            else if (startCellPiece.Type == PieceType.Pawn && _IsPawnPromotionRank(move.EndCell.Y))
            {
                return new PawnPromotionMove();
            }
            // Simple move.
            else
            {
                return new OrdinaryMove();
            }
        }

        // Get a list of possible castles after the move is performed.
        //
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        // castles: A list of possible castles at start/initial board.
        //
        // Returns
        // -------
        // A list of possible castles after the move is performed.
        private static List<Castle> _GetCastlesAfterMove(IBoard board,
                                                         Move move,
                                                         IEnumerable<Castle> castles)
        {
            var nextCastles = castles.ToList();
            var piece = board.GetPiece(move.StartCell).Value;

            // King move.
            if (piece.Type == PieceType.King)
            {
                _CastleConstants.mappingCastleToConstant.Where(pair => pair.Key.Color == piece.Color)
                                                        .ToList()
                                                        .ForEach(pair => nextCastles.Remove(pair.Key));
            }

            // Rook moves.
            _CastleConstants.mappingCastleToConstant.Where(pair => pair.Key.Color == piece.Color
                                                           && move.StartCell == pair.Value.InitialRookCell)
                                                    .ToList()
                                                    .ForEach(pair => nextCastles.Remove(pair.Key));

            // Capture of the enemy rook.
            _CastleConstants.mappingCastleToConstant.Where(pair => pair.Key.Color == piece.Color.Change()
                                                           && move.EndCell == pair.Value.InitialRookCell)
                                                    .ToList()
                                                    .ForEach(pair => nextCastles.Remove(pair.Key));

            return nextCastles;
        }

        // Get en passant cell after the move is performed.
        // 
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        //
        // Returns
        // -------
        // The en passant cell after the move is performed.
        private static Cell? _GetEnPassantCellAfterMove(IBoard board,
                                                        Move move)
        {
            var piece = board.GetPiece(move.StartCell);
            var deltaY = move.EndCell.Y - move.StartCell.Y;

            // Return en passant cell if the pawn moves forward on two cells.
            if (piece?.Type == PieceType.Pawn &&
                Math.Abs(deltaY) == 2)
            {
                var enPassantY = (move.StartCell.Y + move.EndCell.Y) / 2;
                return new Cell(move.StartCell.X, enPassantY);
            }

            return null;
        }
    }
}
