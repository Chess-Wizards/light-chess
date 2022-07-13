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

            var lastPawnRanks = new List<int> { 0, 7 };
            var possiblePromotionPieceTypes = new List<PieceType>{PieceType.Knight,
                                                                  PieceType.Bishop,
                                                                  PieceType.Rook,
                                                                  PieceType.Queen};

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
                {PieceType.Rook, CellsUnderThreat.GetCellsUnderThreatRook},
                {PieceType.Knight, CellsUnderThreat.GetCellsUnderThreatKnight},
                {PieceType.Bishop, CellsUnderThreat.GetCellsUnderThreatBishop},
                {PieceType.Queen, CellsUnderThreat.GetCellsUnderThreatQueen},
                {PieceType.King, CellsUnderThreat.GetCellsUnderThreatKing},
                {PieceType.Pawn, GetNextCellsPawn}
            };

            return mappingPieceTypeToMethod[(PieceType)piece?.Type](cell,
                                            pieceCells,
                                            enemyPieceCells,
                                            board.IsOnBoard,
                                            (Color)piece?.Color)
                // Suggest four moves, if the pawn promotion is applied. Otherwise, only move os suggested.
                .SelectMany(nextCell => ((Piece)piece).Type == PieceType.Pawn && lastPawnRanks.Contains(nextCell.Y)
                                        ? possiblePromotionPieceTypes.Select(pieceType => new Move(cell, nextCell, pieceType))
                                        : new List<Move>() { new Move(cell, nextCell) })
                .ToList();
        }

        // Finds next/move cells produced by pawn at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A list containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A list containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The pawn color.
        //
        // Returns
        // -------
        // A list containing next/move cells produced by pawn at cell |cell|.
        private static List<Cell> GetNextCellsPawn(Cell cell,
                                                   List<Cell> pieceCells,
                                                   List<Cell> enemyPieceCells,
                                                   Func<Cell, bool> IsOnBoard,
                                                   Color activeColor)
        {
            var cellsNext = CellsUnderThreat.GetCellsUnderThreatPawn(cell,
                                                                     pieceCells,
                                                                     enemyPieceCells,
                                                                     IsOnBoard,
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
