using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{

    static public class PieceMoves
    {
        /*
            The class aims to find the array of all moves. Moves cannot be valid.

            This class does not consider checks, en passant moves, and castles.
        */
        
        /// <summary>
        /// Finds moves produced by piece at cell |cell|.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="board">
        /// The board.
        /// </param>
        /// <returns>
        /// A list containing moves produced by piece at cell |cell|. 
        /// </returns>
        public static List<Move> GetMoves(Cell cell, 
                                          StandardBoard board)
        {
            
            var piece = board[cell];
    
            // Return an empty list if the cell is empty.
            if (piece == null)
            {
                return new List<Move>(){};
            }

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: (Color)piece?.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: ((Color)piece?.Color).Change());

            var mappingPieceTypeToMethod = new Dictionary<PieceType, 
                                                          Func<Cell, List<Cell>, 
                                                          List<Cell>, 
                                                          Func<Cell, bool>,
                                                          Color,
                                                          List<Cell>>>()
            {

                {PieceType.Rook, GetNextCellsRook},
                {PieceType.Knight, GetNextCellsKnight},
                {PieceType.Bishop, GetNextCellsBishop},
                {PieceType.Queen, GetNextCellsQueen},
                {PieceType.King, GetNextCellsKing},
                {PieceType.Pawn, GetNextCellsPawn}
            };

            return mappingPieceTypeToMethod[(PieceType)piece?.Type](cell, 
                                            pieceCells, 
                                            enemyPieceCells,
                                            board.OnBoard,
                                            (Color)piece?.Color)
                .Select(nextCell => new Move(cell, nextCell))
                .ToList();
        }

        /// <summary>
        /// Finds next/move cells produced by rook at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as a piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The rook color.
        /// </param>
        /// <returns>
        /// A list containing next/move cells produced by rook at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsRook(Cell cell, 
                                                  List<Cell> pieceCells, 
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color ActiveColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatRook(cell, pieceCells, enemyPieceCells, OnBoard, ActiveColor);
        }

        /// <summary>
        /// Finds next/move cells produced by knight at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="pieceCells">
        /// A list containing pieces which belong to the same color as piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces which belong to enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The knight color.
        /// </param>
        /// <returns>
        /// A list containing next/move cells produced by knight at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsKnight(Cell cell, 
                                                    List<Cell> pieceCells, 
                                                    List<Cell> enemyPieceCells,
                                                    Func<Cell, bool> OnBoard,
                                                    Color ActiveColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatKnight(cell, 
                                                              pieceCells,
                                                              enemyPieceCells, 
                                                              OnBoard, 
                                                              ActiveColor);
        }

        /// <summary>
        /// Finds next/move cells produced by bishop at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces which belong to the same color as piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces which belong to enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The bishop color.
        /// </param>
        /// <returns>
        /// A list containing next/move celss produced by bishop at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsBishop(Cell cell, 
                                                    List<Cell> pieceCells, 
                                                    List<Cell> enemyPieceCells,
                                                    Func<Cell, bool> OnBoard,
                                                    Color ActiveColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatBishop(cell, 
                                                              pieceCells, 
                                                              enemyPieceCells, 
                                                              OnBoard, 
                                                              ActiveColor);
        }

        /// <summary>
        /// Finds next/move cells produced by queen at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces which belong to the same color as piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces which belong to enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The queen color.
        /// </param>
        /// <returns>
        /// A list containing next/move cells produced by queen at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsQueen(Cell cell, 
                                                   List<Cell> pieceCells, 
                                                   List<Cell> enemyPieceCells,
                                                   Func<Cell, bool> OnBoard,
                                                   Color ActiveColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatQueen(cell, 
                                                             pieceCells, 
                                                             enemyPieceCells, 
                                                             OnBoard, 
                                                             ActiveColor);
        }

        /// <summary>
        /// Finds next/move cells produced by king at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces which belong to the same color as piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces which belong to enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The king color.
        /// </param>
        /// <returns>
        /// A list containing next/move cells produced by king at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsKing(Cell cell, 
                                                  List<Cell> pieceCells, 
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color ActiveColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatKing(cell, 
                                                            pieceCells, 
                                                            enemyPieceCells, 
                                                            OnBoard, 
                                                            ActiveColor);
        }

        /// <summary>
        /// Finds next/move cells produced by pawn at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces which belong to the same color as piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces which belong to enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The pawn color.
        /// </param>
        /// <returns>
        /// A list containing next/move cells produced by pawn at cell |cell|. 
        /// </returns>
        public static List<Cell> GetNextCellsPawn(Cell cell, 
                                                  List<Cell> pieceCells, 
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color ActiveColor)
        {
            var cellsNext = CellsUnderThreat.GetCellsUnderThreatPawn(cell, 
                                                                     pieceCells, 
                                                                     enemyPieceCells, 
                                                                     OnBoard, 
                                                                     ActiveColor);

            // Shift depends on color.            
            var shift = ActiveColor == Color.White ? new Cell(0, 1) : new Cell(0, -1);

            var cellOneMoveForward = cell + shift;
            // One move forward.
            if (!pieceCells.Contains(cellOneMoveForward) 
                && !enemyPieceCells.Contains(cellOneMoveForward))
            {
                cellsNext.Add(cellOneMoveForward);
            }

            // Two moves forward.
            var celltwoMovesForward = cellOneMoveForward + shift;
            var startRow = ActiveColor == Color.White ? 1 : 6;
            var notTouchedPawn =  cell.Y == startRow;
            if (notTouchedPawn 
                && !pieceCells.Contains(cellOneMoveForward)
                && !enemyPieceCells.Contains(celltwoMovesForward))
            {
                cellsNext.Add(celltwoMovesForward);
            }

            return cellsNext;
        }
    }
}
