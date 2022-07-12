// This class is not yet done.

using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    public class StandardGame : IGameLogic<StandardGame>
    {
        // This class is the entry point for all commands coming to the GameLogic project.
        // It is responsible for initial instantiation of the game from FEN notation.
        // Game state must be valid!!!

        public StandardGameState gameState;
        public string FENNotation
        {
            get
            {
                return StandardFENSerializer.SerializeToFEN(gameState);
            }
        }

        public StandardGame(string fenNotation)
        {
            this.gameState = StandardFENSerializer.DeserializeFromFEN(fenNotation);
        }

        public StandardGame(StandardGameState gameState)
        {
            this.gameState = gameState;
        }

        // Checks if the mate occurs at the current game state.
        // Enemy color mates/wins the active color.
        // 
        // Returns
        // -------
        // true, if the mate occurs. Otherwise, false.
        public bool IsMate()
        {
            return IsCheck()
                   && FindAllValidMoves().Count == 0;
        }

        // Checks if the check occurs at the current game state.
        // Enemy color checks the active color.
        //
        // Returns
        // -------
        // true, if the check occurs. Otherwise, false.
        public bool IsCheck()
        {
            // Extract king location.
            var kingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                              filterByPieceType: PieceType.King)
                                          .First();

            return FindAllCellsUnderThreat(filterByColor: gameState.EnemyColor).Any(cell => cell == kingCell);
        }

        // Applies the move.
        //
        // Parameters
        // ----------
        // move: The move to apply.
        // 
        // Returns
        // -------
        // A new instance of StandardGame, if the move is valid. Otherwise, returns null.
        public StandardGame? MakeMove(Move move)
        {
            var nextGameState = MoveApplier.GetNextGameState(gameState, move);
            var nextGame = new StandardGame(nextGameState);
            return nextGame.IsValid() ? nextGame : null;
        }

        // Check if the current game state is valid.
        //
        // Returns
        // -------
        // true, if the current game state is valid. Otherwise, returns false.
        public bool IsValid()
        {
            var onlyOneEnemyKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                      filterByPieceType: PieceType.King).Count == 1;

            var onlyOneKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                                 filterByPieceType: PieceType.King).Count == 1;

            var enemyKingCell = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                   filterByPieceType: PieceType.King) // Extract king location.
                                               .First();
            var checkToEnemyKing = FindAllCellsUnderThreat(filterByColor: gameState.ActiveColor).Any(cell => enemyKingCell == cell);

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
        // Returns
        // -------
        // A list containing all moves.
        private List<Move> FindAllMoves()
        {
            return gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor)
                                  .SelectMany(cell => PieceMoves.GetMoves(cell, gameState.Board))
                                  .ToList();
        }

        // Find all valid moves.
        //
        // Returns
        // -------
        // A list containing valid moves.
        public List<Move> FindAllValidMoves()
        {
            return FindAllMoves().Where(move => MakeMove(move) != null)
                                 .ToList();
        }

        // Find all cells 'under threat' produced by |filterByColor| color.
        // 'under threat' means all cells at which the enemy king cannot stand because of the check.
        //
        // Returns
        // -------
        // A list containing all cells.
        private List<Cell> FindAllCellsUnderThreat(Color filterByColor)
        {
            return gameState.Board.GetCellsWithPieces(filterByColor: filterByColor)
                                  .SelectMany(cell => CellsUnderThreat.GetCellsUnderThreat(cell, gameState.Board))
                                  .Distinct()
                                  .ToList();
        }
    }
}
