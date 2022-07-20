using GameLogic.Entities;
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
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // true, if the mate occurs. Otherwise, false.
        public bool IsMate(IStandardGameState gameState)
        {
            return IsCheck(gameState)
                   && FindAllValidMoves(gameState).ToList().Count == 0;
        }

        // Checks if the check occurs at the current game state.
        // Enemy color checks the active color.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // true, if the check occurs. Otherwise, false.
        public bool IsCheck(IStandardGameState gameState)
        {
            // Extract king location.
            var kingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                              filterByPieceType: PieceType.King)
                                          .First();

            return FindAllCellsUnderThreat(gameState, filterByColor: gameState.EnemyColor).Any(cell => cell == kingCell);
        }

        // Applies the move.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        // move: The move to apply.
        // 
        // Returns
        // -------
        // A new instance of StandardGame, if the move is valid. Otherwise, returns null.
        public IStandardGameState? MakeMove(IStandardGameState gameState, Move move)
        {
            if (!IsValid(gameState))
            {
                throw new ArgumentException("Invalid FEN notation.");
            }

            var nextGameState = MoveApplier.GetNextGameState(gameState, move);
            return IsValid(nextGameState) ? nextGameState : null;
        }

        // Check if the current game state is valid.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // true, if the current game state is valid. Otherwise, returns false.
        private bool IsValid(IStandardGameState gameState)
        {
            var onlyOneEnemyKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                      filterByPieceType: PieceType.King).ToList().Count == 1;

            var onlyOneKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                                 filterByPieceType: PieceType.King).ToList().Count == 1;

            var enemyKingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                   filterByPieceType: PieceType.King) // Extract king location.
                                               .First();
            var checkToEnemyKing = FindAllCellsUnderThreat(gameState, filterByColor: gameState.ActiveColor).Any(cell => enemyKingCell == cell);

            // Pawns cannot be located on first and last ranks.
            var invalidPawnRanks = new List<int> { 0, 7 };
            var pawnsOnInvalidRanks = gameState.Board.GetCellsWithPieces(filterByPieceType: PieceType.Pawn)
                                                     .Any(cell => invalidPawnRanks.Contains(cell.Y));

            return onlyOneEnemyKing
                   && onlyOneKing
                   && !checkToEnemyKing
                   && !pawnsOnInvalidRanks;
        }

        // Find all moves. Moves might be not valid.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // A IEnumerable collection containing all moves.
        private IEnumerable<Move> FindAllMoves(IStandardGameState gameState)
        {
            var enPassantMoves = GetEnPassantMoves(gameState).ToList(); ;
            var castleMoves = GetCastleMoves(gameState).ToList(); ;
            var restMoves = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor)
                                           .SelectMany(cell => PieceMoves.GetMoves(cell, gameState.Board))
                                           .ToList();
            // Combine moves.
            return enPassantMoves.Concat(castleMoves)
                                 .Concat(restMoves);
        }

        // Find all en passant moves.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // A IEnumerable collection containing en passant moves.
        private IEnumerable<Move> GetEnPassantMoves(IStandardGameState gameState)
        {
            var enPassantMoves = new List<Move>() { };

            if (gameState.EnPassantCell != null)
            {
                var yShift = gameState.ActiveColor == Color.White ? -1 : 1;
                var xShifts = new List<int>() { -1, 1 };

                // Check possible pawns which can perform an en passant move.
                enPassantMoves = xShifts.Select(xShift => (Cell)gameState.EnPassantCell + new Cell(xShift, yShift))
                                        .Where(cell => gameState.Board.IsOnBoard(cell)
                                                       && !gameState.Board.IsEmpty(cell)
                                                       && gameState.Board.GetPiece(cell)?.Type == PieceType.Pawn
                                                       && gameState.Board.GetPiece(cell)?.Color == gameState.ActiveColor)
                                        .Select(cell => new Move(cell, (Cell)gameState.EnPassantCell))
                                        .ToList();
            }

            return enPassantMoves;
        }

        // Find all castle moves.
        //
        // This function should not check the location of the rook and king.
        // gameState contains this information implicitly in |AvailableCastles| field.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // A IEnumerable collection containing castle moves.
        private IEnumerable<Move> GetCastleMoves(IStandardGameState gameState)
        {
            var mappingCatleToMoveNotation = new Dictionary<Castle, string>
            {
                {new Castle(Color.White, CastleType.King), "e1g1"},
                {new Castle(Color.White, CastleType.Queen), "e1c1"},
                {new Castle(Color.Black, CastleType.King), "e8g8"},
                {new Castle(Color.Black, CastleType.Queen), "e8c8"}
            };
            var emptyCellNotations = new Dictionary<Castle, IEnumerable<string>>
            {
                {new Castle(Color.White, CastleType.King), new List<string>(){"f1", "g1"}},
                {new Castle(Color.White, CastleType.Queen), new List<string>(){"b1", "c1", "d1"}},
                {new Castle(Color.Black, CastleType.King), new List<string>(){"f8", "g8"}},
                {new Castle(Color.Black, CastleType.Queen), new List<string>(){"b8", "c8", "d8"}}
            };

            return gameState.AvailableCastles
                            .Where(castle => castle.Color == gameState.ActiveColor
                                             && emptyCellNotations[castle].All(notation => gameState.Board.IsEmpty((Cell)StandardFENSerializer.NotationToCell(notation))))
                            .Select(castle => StandardFENSerializer.NotationToMove(mappingCatleToMoveNotation[castle]));
        }

        // Find all valid moves.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // A IEnumerable collection containing valid moves.
        public IEnumerable<Move> FindAllValidMoves(IStandardGameState gameState)
        {
            if (!IsValid(gameState))
            {
                throw new ArgumentException("Invalid FEN notation.");
            }

            return FindAllMoves(gameState).Where(move => MakeMove(gameState, move) != null);
        }

        // Find all cells 'under threat' produced by |filterByColor| color.
        // 'under threat' means all cells at which the enemy king cannot stand because of the check.
        //
        // Parameters
        // ----------
        // gameState: The state of the game.
        //
        // Returns
        // -------
        // A IEnumerable collection containing all cells.
        private IEnumerable<Cell> FindAllCellsUnderThreat(IStandardGameState gameState, Color filterByColor)
        {
            return gameState.Board.GetCellsWithPieces(filterByColor: filterByColor)
                                  .SelectMany(cell => CellsUnderThreat.GetCellsUnderThreat(cell, gameState.Board))
                                  .Distinct();
        }
    }
}
