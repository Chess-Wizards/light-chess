// This class is not yet done.

using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    public class StandardGame : IStandardGameLogic, IFENSerializable<StandardGameState>
    {
        /*
        This class contains a game logic.
        */

        /// <summary>
        /// Serialize game object to FEN notation.
        /// </summary>
        /// <param name="objectToSerialize">
        /// The game to serialize.
        /// </param>        
        /// <returns>
        /// The game FEN notation. 
        /// </returns>        
        public string SerializeToFEN(StandardGameState objectToSerialize)
        {
            var splitFenNotation = new string[6]
            {
            SerializeHelper.BoardToNotation(objectToSerialize.Board),
            SerializeHelper.ColorToNotation(objectToSerialize.ActiveColor),
            SerializeHelper.CastleToNotation(objectToSerialize.AvaialbleCastleMoves),
            SerializeHelper.CellToNotation(objectToSerialize.EnPassantCell),
            objectToSerialize.HalfmoveNumber.ToString(),
            objectToSerialize.FullmoveNumber.ToString()
            };

            return String.Join(" ", splitFenNotation);
        }

        /// <summary>
        /// Deserialize FEN notation to game object.
        /// </summary>
        /// <param name="fenNotation">
        /// The game FEN notation.
        /// </param>        
        /// <exception 
        ///cref="ArgumentException">Invalid |fenNotation|.
        ///</exception>
        /// <returns>
        /// The game. 
        /// </returns>  
        public StandardGameState DeserializeFromFEN(string fenNotation)
        {
            var splitFenNotation = fenNotation.Split(' ');

            if (splitFenNotation.Count() != 6)
                throw new ArgumentException("Invalid FEN notation.");

            var gameState = new StandardGameState(
                SerializeHelper.NotationToBoard(splitFenNotation[0]),
                SerializeHelper.NotationToColor(splitFenNotation[1]),
                SerializeHelper.NotationToCastle(splitFenNotation[2]),
                SerializeHelper.NotationToCell(splitFenNotation[3]),
                Int32.Parse(splitFenNotation[4]),
                Int32.Parse(splitFenNotation[5])
            );
            return gameState;
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
