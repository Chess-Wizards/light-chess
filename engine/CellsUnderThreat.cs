using System;
using System.Linq;
using System.Collections.Generic;

namespace LightChess
{

    static public class CellsUnderThreat
    {
        /*
            The static class aims to find a list of cells 'under threat'. 'under threat' means all cells
            at which the enemy king cannot stand because of the check. In addition, the piece must be able to 
            make a move at this cell. For example, a pawn can capture iff the enemy piece stands at a diagonal.

            This class does not consider checks, en passant moves, and castles.
        */


        /// <summary>
        /// Finds cells under threat produced by piece at cell |cell|.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="board">
        /// The board.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by piece at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreat(Cell cell, 
                                                     StandardBoard board)
        {

            var piece = board[cell];
            
            // Return empty list if cell is empty.
            if (piece == null)
            {
                return new List<Cell>(){};
            }

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: (Color)piece?.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: ((Color)piece?.Color).Change());

            var mappingPieceTypeToMethod = new Dictionary<PieceType, 
                                                          Func<Cell, 
                                                          List<Cell>, 
                                                          List<Cell>, 
                                                          Func<Cell, bool>,
                                                          Color, 
                                                          List<Cell>>>()
            {
                {PieceType.Rook, GetCellsUnderThreatRook},
                {PieceType.Knight, GetCellsUnderThreatKnight},
                {PieceType.Bishop, GetCellsUnderThreatBishop},
                {PieceType.Queen, GetCellsUnderThreatQueen},
                {PieceType.King, GetCellsUnderThreatKing},
                {PieceType.Pawn, GetCellsUnderThreatPawn}
            };

            return mappingPieceTypeToMethod[(PieceType)piece?.Type](cell, 
                                            pieceCells, 
                                            enemyPieceCells,
                                            board.OnBoard,
                                            (Color)piece?.Color);


        }

        /// <summary>
        /// Finds cells under threat produced by piece at cell |cell|. 
        /// This function is a helper one for other methods.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="shifts">
        /// A list containing possible cell shifts.
        /// </param>
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="oneShift">
        /// Check only |shifts| if true, otherwise perform shifts until the cells are on board.
        /// For example, |oneShift| is true for bishop, rook, and queen, while false for pawn, knight, and king.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by piece at cell |cell|. 
        /// </returns>
        public static List<Cell> FindCells(Cell cell, 
                                           List<Cell> shifts, 
                                           List<Cell> pieceCells, 
                                           List<Cell> enemyPieceCells,
                                           Func<Cell, bool> OnBoard,
                                           bool oneShift=false)
        {
            // List to save cells 'under threat'
            var cellsUnderThreat = new List<Cell>();

            // Iterate over shifts
            foreach (var shift in shifts)
            {
                var currentCell = cell;
                // Iterate once if |oneShift| is set to true, otherwise iterate until obstacles are found.
                while (true)
                {
                    currentCell = currentCell + shift;
                    if (!OnBoard(currentCell) 
                        || pieceCells.Contains(currentCell)) break;

                    cellsUnderThreat.Add(currentCell);

                    if (oneShift 
                        || enemyPieceCells.Contains(currentCell)) break;
                }
            }
            return cellsUnderThreat;
        }

        /// <summary>
        /// Finds cells under threat produced by rook at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
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
        /// A list containing cells under threat produced by rook at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatRook(Cell cell, 
                                                         List<Cell> pieceCells, 
                                                         List<Cell> enemyPieceCells,
                                                         Func<Cell, bool> OnBoard,
                                                         Color ActiveColor)
        {
            // up and down - y-axis or height
            // right and left - x-axis or width
            var upShift = new Cell(0, 1);
            var rightShift = new Cell(1, 0);
            var downShift = new Cell(0, -1);
            var leftShift = new Cell(-1, 0);
            
            var shifts = new List<Cell>()
            {   
                upShift, 
                rightShift, 
                downShift, 
                leftShift
            };
            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             OnBoard,
                             oneShift:false);
        }

        /// <summary>
        /// Finds cells under threat produced by knight at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The knight color.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by knight at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatKnight(Cell cell, 
                                                           List<Cell> pieceCells, 
                                                           List<Cell> enemyPieceCells,
                                                           Func<Cell, bool> OnBoard,
                                                           Color ActiveColor)
        {
            var xs = new[] {-2, -1, 1, 2};
            var ys = new[] {-2, -1, 1, 2};  
            var shifts = 
                (from x in xs 
                from y in ys 
                select new { x, y})
                .Where((tuple) => Math.Abs(tuple.x) != Math.Abs(tuple.y))
                .Select((tuple) => new Cell(tuple.x, tuple.y))
                .ToList();

            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             OnBoard,
                             oneShift:true);

        }

        /// <summary>
        /// Finds cells under threat produced by bishop at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The bishop color.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by bishop at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatBishop(Cell cell, 
                                                           List<Cell> pieceCells, 
                                                           List<Cell> enemyPieceCells,
                                                           Func<Cell, bool> OnBoard,
                                                           Color ActiveColor)
        
        {
            // up and down - y-axis or height.
            // right and left - x-axis or width.
            var upRightShift = new Cell(1, 1);
            var downRightShift = new Cell(1, -1);
            var downLeftShift = new Cell(-1, -1);
            var upLeftShift = new Cell(-1, 1);
            
            var shifts = new List<Cell>()
            {
                upRightShift, 
                downRightShift, 
                downLeftShift, 
                upLeftShift
            };
            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             OnBoard,
                             oneShift:false);
        }

        /// <summary>
        /// Finds cells under threat produced by queen at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The queen color.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by queen at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatQueen(Cell cell, 
                                                          List<Cell> pieceCells, 
                                                          List<Cell> enemyPieceCells,
                                                          Func<Cell, bool> OnBoard,
                                                          Color ActiveColor)
        
        {
            var cellsUnderThreatRook = GetCellsUnderThreatRook(cell, 
                                                               pieceCells, 
                                                               enemyPieceCells, 
                                                               OnBoard, 
                                                               ActiveColor);
            var cellsUnderThreatBishop = GetCellsUnderThreatBishop(cell, 
                                                                   pieceCells, 
                                                                   enemyPieceCells, 
                                                                   OnBoard, 
                                                                   ActiveColor);
            // Concatenate/combine rook and bishop cells. 
            return cellsUnderThreatRook.Concat(cellsUnderThreatBishop).ToList();;
        }

        /// <summary>
        /// Finds cells under threat produced by king at cell |cell|. 
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// <param name="pieceCells">
        /// A list containing pieces that belong to the same color as the piece at cell |cell|.
        /// </param>
        /// <param name="enemyPieceCells">
        /// A list containing pieces that belong to the enemy color.
        /// </param>
        /// <param name="OnBoard">
        /// A function to decide on where the cell is on board.
        /// </param>
        /// <param name="ActiveColor">
        ///  The king color.
        /// </param>
        /// <returns>
        /// A list containing cells under threat produced by king at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatKing(Cell cell, 
                                                         List<Cell> pieceCells, 
                                                         List<Cell> enemyPieceCells,
                                                         Func<Cell, bool> OnBoard,
                                                         Color ActiveColor)
        {    
            var xs = new[] {-1, 0, 1};
            var ys = new[] {-1, 0, 1};  
            var shifts = 
                (from x in xs 
                from y in ys 
                select new {x, y}) // Get all combinations.
                .Where((tuple) => !(tuple.x == 0 && tuple.y == 0)) // Exclude tuple corresponding to |cell|.
                .Select((tuple) => new Cell(tuple.x, tuple.y))
                .ToList();

            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             OnBoard,
                             oneShift:true);

        }

        /// <summary>
        /// Finds cells under threat produced by pawn at cell |cell|. 
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
        /// A list containing cells under threat produced by pawn at cell |cell|. 
        /// </returns>
        public static List<Cell> GetCellsUnderThreatPawn(Cell cell, 
                                                         List<Cell> pieceCells, 
                                                         List<Cell> enemyPieceCells,
                                                         Func<Cell, bool> OnBoard,
                                                         Color ActiveColor)
        {

            var leftShift = ActiveColor == Color.White ?  new Cell(-1, 1) : new Cell(-1, -1); 
            var rightShift = ActiveColor == Color.White ?  new Cell(1, 1) : new Cell(1, -1);

            // A list to save cells 'under threat'
            var cellsUnderThreat = new List<Cell>();

            // Iterate over shifts
            foreach (var shift in new Cell[]{leftShift, rightShift})
            {
                var currentCell = cell + shift;
                if (enemyPieceCells.Contains(currentCell))
                     cellsUnderThreat.Add(currentCell);
            }
            return cellsUnderThreat;
        }
    }

}
