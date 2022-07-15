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

        public StandardGame(string fenNotation)
        {
            this.gameState = StandardFENSerializer.DeserializeFromFEN(fenNotation);
            if (!IsValid())
            {
                throw new ArgumentException("Invalid input FEN notation.");
            }
        }

        private StandardGame(StandardGameState gameState)
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
        private bool IsValid()
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
            var enPassantMoves = GetEnPassantMoves();
            var castleMoves = GetCastleMoves();
            var restMoves = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor)
                                           .SelectMany(cell => PieceMoves.GetMoves(cell, gameState.Board))
                                           .ToList();
            // Combine moves.
            return enPassantMoves.Concat(castleMoves)
                                 .Concat(restMoves)
                                 .ToList();
        }

        // Find all en passant moves.
        //
        // Returns
        // -------
        // A list containing en passant moves.
        private List<Move> GetEnPassantMoves()
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
                                                       && gameState.Board[cell]?.Type == PieceType.Pawn
                                                       && gameState.Board[cell]?.Color == gameState.ActiveColor)
                                        .Select(cell => new Move(cell, (Cell)gameState.EnPassantCell))
                                        .ToList();
            }

            return enPassantMoves;
        }

        // Find all castle moves.
        //
        // This function should not check the location of the rook and king.
        // gameState contains this information implicitly in |AvaialbleCastles| field.
        //
        // Returns
        // -------
        // A list containing castle moves.
        private List<Move> GetCastleMoves()
        {
            var mappingCatleToMoveNotation = new Dictionary<Castle, string>
            {
                {new Castle(Color.White, CastleType.King), "e1g1"},
                {new Castle(Color.White, CastleType.Queen), "e1c1"},
                {new Castle(Color.Black, CastleType.King), "e8g8"},
                {new Castle(Color.Black, CastleType.Queen), "e8c8"}
            };
            var emptyCellNotations = new Dictionary<Castle, List<string>>
            {
                {new Castle(Color.White, CastleType.King), new List<string>(){"f1", "g1"}},
                {new Castle(Color.White, CastleType.Queen), new List<string>(){"b1", "c1", "d1"}},
                {new Castle(Color.Black, CastleType.King), new List<string>(){"f8", "g8"}},
                {new Castle(Color.Black, CastleType.Queen), new List<string>(){"b8", "c8", "d8"}}
            };

            return gameState.AvaialbleCastles
                            .Where(castle => castle.Color == gameState.ActiveColor
                                             && emptyCellNotations[castle].All(notation => gameState.Board.IsEmpty((Cell)StandardFENSerializer.NotationToCell(notation))))
                            .Select(castle => StandardFENSerializer.NotationToMove(mappingCatleToMoveNotation[castle]))
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
