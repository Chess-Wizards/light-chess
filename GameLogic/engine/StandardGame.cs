// This class is not yet done.

using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    public class StandardGame : IStandardGameLogic, IFENSerializable<StandardGameState>
    {
        // This class is the entry point for all commands coming to the GameLogic project.
        // It is responsible for initial instantiation of the game from FEN notation.

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
            gameState = StandardFENSerializer.DeserializeFromFEN(fenNotation);
        }

        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public bool IsMate(StandardGameState gameState)
        {
            foreach (var move in FindAllMoves(gameState))
            {
                var nextGameState = MakeMove(gameState, move);
                if (!IsCheck(nextGameState))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public bool IsCheck(StandardGameState gameState)
        {
            // Extract king location(s). 
            var kingCells = gameState.Board.GetCellsWithPieces(filterByColor: gameState.ActiveColor,
                                                               filterByPieceType: PieceType.King);
            if (kingCells.Count != 1)
            {
                throw new InvalidOperationException("Invalid game state. There is only one king on standard board.");
            }

            // Extract only one king cell.
            var kingCell = kingCells[0];

            foreach (var cell in FindAllCellsUnderThreat(gameState))
            {
                if (cell.Equals(kingCell)) return true;
            }
            return false;
        }


        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public StandardGameState? MakeMove(StandardGameState gameState, Move move)
        {
            var nextGameState = MoveApplier.GetNextGameState(gameState, move);
            return (IsGameStateValid(nextGameState)) ? nextGameState : null;
        }

        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public bool IsGameStateValid(StandardGameState gameState)
        {
            var onlyOneEnemyKing = gameState.Board.GetCellsWithPieces(filterByColor: gameState.EnemyColor,
                                                                      filterByPieceType: PieceType.King).Count == 1;
            return onlyOneEnemyKing && !IsCheck(gameState);
        }

        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public List<Move> FindAllMoves(StandardGameState gameState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: not finished.
        /// </summary>
        public List<Cell> FindAllCellsUnderThreat(StandardGameState gameState)
        {
            throw new NotImplementedException();
        }

        // public List<Move> FindAllMoves(StandardGameState gameState)
        // {
        //     var cellsWithEnemyPieces = gameState.Board.GetCellsWithPieces(filterByColor:gameState.ActiveColor,
        //                                                                   filterByPieceType:PieceType.King);
        //     var items = new List<T>();
        //     // Iterate over enemy cells/pieces. Per each iteration find
        //     // items produced by enemy cell/piece.
        //     foreach(var cell in cellsWithEnemyPieces)
        //     {
        //         var piece = (Piece)gameState.Board[cell];
        //         items.AddRange(Get(piece, cell, gameState));
        //     }
        //     return items;



        //     return FindAll<Move>(gameState, PieceMoves.GetMoves);
        // }

        // public List<Cell> FindAllCellsUndeThreat(StandardGameState gameState)
        // {
        //     return FindAll<Cell>(gameState, CellsUnderThreat.GetCellsUnderThreat);
        // }

        // public List<T> FindAll<T>(StandardGameState gameState, Func<Piece, Cell, StandardGameState, List<T>> Get)
        // {            
        //     var cellsWithEnemyPieces = gameState.Board.GetCellsWithPieces(filterByColor:gameState.EnemyColor,
        //                                                                   filterByPieceType:PieceType.King);
        //     var items = new List<T>();
        //     // Iterate over enemy cells/pieces. Per each iteration find
        //     // items produced by enemy cell/piece.
        //     foreach(var cell in cellsWithEnemyPieces)
        //     {
        //         var piece = (Piece)gameState.Board[cell];
        //         items.AddRange(Get(piece, cell, gameState));
        //     }
        //     return items;

        // }

    }
}
