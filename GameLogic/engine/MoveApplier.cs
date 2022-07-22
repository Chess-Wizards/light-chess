using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.States;
using GameLogic.Engine.MoveTypes;

namespace GameLogic.Engine
{
    // Applies a move.
    public static class MoveApplier
    {

        private static readonly List<int> _LastPawnRanks = new List<int>() { 0, 7 };

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

        private static bool _IsLastRank(int rank)
        {
            return _LastPawnRanks.Contains(rank);
        }

        private static IMoveType<IRectangularBoard> _SelectMoveType(IStandardGameState gameState, Piece startCellPiece, Move move)
        {
            var deltaX = Math.Abs(move.EndCell.X - move.StartCell.X);
            if (startCellPiece.Type == PieceType.King && deltaX == 2)
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
            // else if (startCellPiece.Type == PieceType.Pawn && _IsLastRank(move.EndCell.Y))
            else if (startCellPiece.Type == PieceType.Pawn && _IsLastRank(move.EndCell.Y))
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
            var nextCastles = new List<Castle>(castles);
            var piece = board.GetPiece(move.StartCell).Value;
            // Castles.
            var kingCastle = new Castle(piece.Color, CastleType.King);
            var queenCastle = new Castle(piece.Color, CastleType.Queen);
            // Enemy castles.
            var kingEnemyCastle = new Castle(piece.Color.Change(), CastleType.King);
            var queenEnemyCastle = new Castle(piece.Color.Change(), CastleType.Queen);
            // Initial rook cells.
            var y = piece.Color == Color.White ? 0 : 7;
            var kingRookInitialCell = new Cell(7, y);
            var queenRookInitialCell = new Cell(0, y);
            // Initial enemy rook cells.
            var yEnemy = piece.Color == Color.White ? 7 : 0;
            var kingEnemyRookInitialCell = new Cell(7, yEnemy);
            var queenEnemyRookInitialCell = new Cell(0, yEnemy);

            // Update castles by removing castles.
            if (piece.Type == PieceType.King)
            {
                nextCastles.Remove(kingCastle);
                nextCastles.Remove(queenCastle);
            }
            else if (move.StartCell == kingRookInitialCell)
            {
                nextCastles.Remove(kingCastle);
            }
            else if (move.StartCell == queenRookInitialCell)
            {
                nextCastles.Remove(queenCastle);
            }

            // Update castles by removing enemy castles.
            if (move.EndCell == kingEnemyRookInitialCell)
            {
                nextCastles.Remove(kingEnemyCastle);
            }
            else if (move.EndCell == queenEnemyRookInitialCell)
            {
                nextCastles.Remove(queenEnemyCastle);
            }

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
