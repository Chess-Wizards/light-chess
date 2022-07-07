using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    // The class aims to find the array of all moves. Moves cannot be valid.
    // This class does not consider checks, en passant moves, and castles.
    static public class PieceMoves
    {
        // Finds moves produced by piece at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // board: The board represents the current arrangement of all pieces.
        //
        // Returns
        // -------
        // A list containing moves produced by piece at cell |cell|.   
        public static List<Move> GetMoves(Cell cell,
                                          StandardBoard board)
        {
            var piece = board[cell];

            // Return an empty list if the cell is empty.
            if (piece == null)
            {
                return new List<Move>() { };
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

        // Finds next/move cells produced by rook at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The rook color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by rook at cell |cell|. 
        public static List<Cell> GetNextCellsRook(Cell cell,
                                                  List<Cell> pieceCells,
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color activeColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatRook(cell,
                                                            pieceCells,
                                                            enemyPieceCells,
                                                            OnBoard,
                                                            activeColor);
        }

        // Finds next/move cells produced by knight at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The knight color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by knight at cell |cell|.
        public static List<Cell> GetNextCellsKnight(Cell cell,
                                                    List<Cell> pieceCells,
                                                    List<Cell> enemyPieceCells,
                                                    Func<Cell, bool> OnBoard,
                                                    Color activeColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatKnight(cell,
                                                              pieceCells,
                                                              enemyPieceCells,
                                                              OnBoard,
                                                              activeColor);
        }

        // Finds next/move cells produced by bishop at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The bishop color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by bishop at cell |cell|.
        public static List<Cell> GetNextCellsBishop(Cell cell,
                                                    List<Cell> pieceCells,
                                                    List<Cell> enemyPieceCells,
                                                    Func<Cell, bool> OnBoard,
                                                    Color activeColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatBishop(cell,
                                                              pieceCells,
                                                              enemyPieceCells,
                                                              OnBoard,
                                                              activeColor);
        }

        // Finds next/move cells produced by queen at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The queen color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by queen at cell |cell|.
        public static List<Cell> GetNextCellsQueen(Cell cell,
                                                   List<Cell> pieceCells,
                                                   List<Cell> enemyPieceCells,
                                                   Func<Cell, bool> OnBoard,
                                                   Color activeColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatQueen(cell,
                                                             pieceCells,
                                                             enemyPieceCells,
                                                             OnBoard,
                                                             activeColor);
        }

        // Finds next/move cells produced by king at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The king color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by king at cell |cell|.
        public static List<Cell> GetNextCellsKing(Cell cell,
                                                  List<Cell> pieceCells,
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color activeColor)
        {
            return CellsUnderThreat.GetCellsUnderThreatKing(cell,
                                                            pieceCells,
                                                            enemyPieceCells,
                                                            OnBoard,
                                                            activeColor);
        }

        // Finds next/move cells produced by pawn at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // OnBoard: A function to decide on where the cell is on board.
        // activeColor: The pawn color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by pawn at cell |cell|.
        public static List<Cell> GetNextCellsPawn(Cell cell,
                                                  List<Cell> pieceCells,
                                                  List<Cell> enemyPieceCells,
                                                  Func<Cell, bool> OnBoard,
                                                  Color activeColor)
        {
            var cellsNext = CellsUnderThreat.GetCellsUnderThreatPawn(cell,
                                                                     pieceCells,
                                                                     enemyPieceCells,
                                                                     OnBoard,
                                                                     activeColor);

            // Shift depends on color.            
            var shift = activeColor == Color.White ? new Cell(0, 1) : new Cell(0, -1);

            var cellOneMoveForward = cell + shift;
            // One move forward.
            if (!pieceCells.Contains(cellOneMoveForward)
                && !enemyPieceCells.Contains(cellOneMoveForward))
            {
                cellsNext.Add(cellOneMoveForward);
            }

            // Two moves forward.
            var celltwoMovesForward = cellOneMoveForward + shift;
            var startRow = activeColor == Color.White ? 1 : 6;
            var notTouchedPawn = cell.Y == startRow;
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
