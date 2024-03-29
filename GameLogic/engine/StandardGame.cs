using GameLogic.Engine.Moves;
using GameLogic.Entities;
using GameLogic.Entities.Castlings;
using GameLogic.Entities.Pieces;
using GameLogic.Entities.States;

namespace GameLogic.Engine
{
    // This class is the entry point for all commands coming to the GameLogic project.
    // It is responsible for initial instantiation of the game.
    // Game state must be valid!!!
    public class StandardGame : IGameLogic<IStandardGameState>
    {
        // Checks if the mate occurs at the current game state.
        // Enemy color mates/wins the active color.
        public bool IsMate(IStandardGameState gameState)
        {
            return IsCheck(gameState) && !FindAllValidMoves(gameState).Any();
        }

        // Checks if the check occurs at the current game state.
        // Enemy color checks the active color.
        public bool IsCheck(IStandardGameState gameState)
        {
            // Extract king location.
            var kingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                              filterByPieceType: PieceType.King).First();

            return _FindAllCellsUnderThreat(gameState, filterByColor: gameState.EnemyColor).Any(cell => cell == kingCell);
        }

        // Applies the move.
        public IStandardGameState? MakeMove(IStandardGameState gameState, Move move)
        {
            if (!_IsValid(gameState))
            {
                throw new ArgumentException("Invalid game state.");
            }

            var nextGameState = gameState.ApplyMove(move);
            return _IsValid(nextGameState) ? nextGameState : null;
        }

        // Checks if the current game state is valid.
        private static bool _IsValid(IStandardGameState gameState)
        {
            var onlyOneEnemyKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                      filterByPieceType: PieceType.King).Count() == 1;

            var onlyOneKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                                 filterByPieceType: PieceType.King).Count() == 1;

            var enemyKingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                   filterByPieceType: PieceType.King) // Extract king location.
                                               .First();
            var checkToEnemyKing = _FindAllCellsUnderThreat(gameState, filterByColor: gameState.ActiveColor).Any(cell => enemyKingCell == cell);

            // Pawns cannot be located on first and last ranks.
            var pawnsOnInvalidRanks = gameState.Board.GetCellsWithPieces(filterByPieceType: PieceType.Pawn)
                                                     .Any(cell => PieceConstants.InvalidPawnRanks.Contains(cell.Y));

            return onlyOneEnemyKing
                   && onlyOneKing
                   && !checkToEnemyKing
                   && !pawnsOnInvalidRanks;
        }

        // Finds all moves. Moves might be invalid.
        private static IEnumerable<Move> _FindAllMoves(IStandardGameState gameState)
        {
            var enPassantMoves = _GetEnPassantMoves(gameState);
            var castleMoves = _GetCastlingMoves(gameState);
            var restMoves = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor)
                                           .SelectMany(cell => PieceMoves.GetMoves(cell, gameState.Board));

            // Combine moves.
            return enPassantMoves.Concat(castleMoves)
                                 .Concat(restMoves);
        }

        // Finds all en passant moves.
        private static IEnumerable<Move> _GetEnPassantMoves(IStandardGameState gameState)
        {
            var enPassantMoves = Enumerable.Empty<Move>();

            if (gameState.EnPassantCell != null)
            {
                // Check possible pawns which can perform an en passant move.
                enPassantMoves = PieceConstants.ShiftsForEnPassantMove[gameState.ActiveColor]
                                               .Select(shift => gameState.EnPassantCell.Value - shift)
                                               .Where(cell => gameState.Board.IsOnBoard(cell))
                                               .Where(cell => !gameState.Board.IsEmpty(cell))
                                               .Where(cell => gameState.Board.GetPiece(cell)?.Type == PieceType.Pawn)
                                               .Where(cell => gameState.Board.GetPiece(cell)?.Color == gameState.ActiveColor)
                                               .Select(cell => new Move(cell, (Cell)gameState.EnPassantCell));
            }

            return enPassantMoves;
        }

        // Finds all castlings.
        //
        // This function should not check the location of the rook and king.
        // gameState contains this information implicitly in |AvailableCastlings| field.
        private static IEnumerable<Move> _GetCastlingMoves(IStandardGameState gameState)
        {
            return gameState.AvailableCastlings
                            .Where(castling => castling.Color == gameState.ActiveColor)
                            .Where(castling => CastlingConstants.castlingToConstantsMap[castling].RequiredEmptyCells.All(cell => gameState.Board.IsEmpty(cell)))
                            .Select(castling => CastlingConstants.castlingToConstantsMap[castling].CastlingMove);
        }

        // Finds all valid moves.
        public IEnumerable<Move> FindAllValidMoves(IStandardGameState gameState)
        {
            if (!_IsValid(gameState))
            {
                throw new ArgumentException("Invalid game state.");
            }

            return _FindAllMoves(gameState).Where(move => MakeMove(gameState, move) != null);
        }

        // Finds all cells 'under threat' produced by |filterByColor| color.
        // 'under threat' means all cells at which the enemy king cannot stand because of the check.
        private static IEnumerable<Cell> _FindAllCellsUnderThreat(IStandardGameState gameState, Color filterByColor)
        {
            return gameState.Board.GetCellsWithPieces(filterByColor: filterByColor)
                                  .SelectMany(cell => CellsUnderThreat.GetCellsUnderThreat(cell, gameState.Board))
                                  .Distinct();
        }
    }
}
