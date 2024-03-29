using GameLogic.Engine.MoveTypes;
using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Castlings;
using GameLogic.Entities.Pieces;
using GameLogic.Entities.States;

namespace GameLogic.Engine
{
    // Applies a move.
    public static class MoveApplier
    {
        // Applies move and returns next game state.
        public static IStandardGameState ApplyMove(this IStandardGameState gameState, Move move)
        {
            var optionalPiece = gameState.Board.GetPiece(move.StartCell);
            if (optionalPiece == null)
            {
                throw new ArgumentException("The start/initial cell doesn't contain a piece.");
            }

            var piece = optionalPiece.Value;
            var moveType = _SelectMoveType(gameState, piece, move);
            var nextBoard = moveType.Apply(gameState.Board, move);

            // Next castlings.
            var nextAvailableCastlings = _GetCastlingsAfterMove(gameState.Board, move, gameState.AvailableCastlings);

            // Next cells. 
            var nextEnPassantCell = _GetEnPassantCellAfterMove(gameState.Board, move);

            // Next HalfmoveNumber.
            var movePawn = (piece.Type == PieceType.Pawn);
            var moveCapture = !gameState.Board.IsEmpty(move.EndCell) || move.EndCell == gameState.EnPassantCell;
            var nextHalfmoveNumber = movePawn || moveCapture ? 0 : gameState.HalfmoveNumber + 1;

            // Next FullmoveNumber.
            var nextFullmoveNumber = gameState.FullmoveNumber + 1;

            return new StandardGameState(
                nextBoard,
                gameState.EnemyColor,
                nextAvailableCastlings,
                nextEnPassantCell,
                nextHalfmoveNumber,
                nextFullmoveNumber
            );
        }

        private static bool _IsPawnPromotionRank(int rank)
        {
            return PieceConstants.BlackPawnPromotionRank == rank ||
                PieceConstants.WhitePawnPromotionRank == rank;
        }

        private static IMoveType<IRectangularBoard> _SelectMoveType(IStandardGameState gameState, Piece startCellPiece, Move move)
        {
            if (startCellPiece.Type == PieceType.King
                && CastlingConstants.castlingToConstantsMap.Values.Any(castleConstant => castleConstant.CastlingMove == move))
            {
                return new CastlingMove();
            }
            // En passant move.
            else if (gameState.EnPassantCell != null
                    && move.EndCell == (Cell)gameState.EnPassantCell)
            {
                return new EnPassantMove();
            }
            // Pawn promotion.
            else if (startCellPiece.Type == PieceType.Pawn
                    && _IsPawnPromotionRank(move.EndCell.Y))
            {
                return new PawnPromotionMove();
            }
            // Simple move.
            else
            {
                return new OrdinaryMove();
            }
        }

        // Gets a list of possible castles after the move is performed.
        private static IList<Castling> _GetCastlingsAfterMove(IBoard board, Move move,
                                                              IEnumerable<Castling> castles)
        {
            var nextCastles = castles.ToList();
            var piece = board.GetPiece(move.StartCell).Value; // TODO: CS8629

            // King move.
            if (piece.Type == PieceType.King)
            {
                CastlingConstants.castlingToConstantsMap.Where(pair => pair.Key.Color == piece.Color)
                                                        .ToList()
                                                        .ForEach(pair => nextCastles.Remove(pair.Key));
            }

            // Rook moves.
            CastlingConstants.castlingToConstantsMap.Where(pair => pair.Key.Color == piece.Color)
                                                    .Where(pair => move.StartCell == pair.Value.InitialRookCell)
                                                    .ToList()
                                                    .ForEach(pair => nextCastles.Remove(pair.Key));

            // Capture of the enemy rook.
            CastlingConstants.castlingToConstantsMap.Where(pair => pair.Key.Color == piece.Color.Inversed())
                                                    .Where(pair => move.EndCell == pair.Value.InitialRookCell)
                                                    .ToList()
                                                    .ForEach(pair => nextCastles.Remove(pair.Key));

            return nextCastles;
        }

        // Get en passant cell after the move is performed.
        private static Cell? _GetEnPassantCellAfterMove(IBoard board, Move move)
        {
            var piece = board.GetPiece(move.StartCell).Value; // TODO: CS8629
            var deltaY = move.EndCell.Y - move.StartCell.Y;

            // Return en passant cell if the pawn moves forward on two cells.
            if (piece.Type == PieceType.Pawn &&
                Math.Abs(deltaY) == PieceConstants.MaxForwardPawnMovesNotTouched)
            {
                return move.EndCell + PieceConstants.NewEnPassantCellAfterMove[piece.Color];
            }

            return null;
        }
    }
}
