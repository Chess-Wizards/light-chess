using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine
{
    // Finds a INumerable of cells 'under threat'. 'under threat' means all cells
    // at which the enemy king cannot stand because of the check. In addition, the piece must be able to 
    // make a move at this cell. For example, a pawn can capture iff the enemy piece stands at a diagonal.
    //
    // This class does not consider checks, en passant moves, and castles.
    static public class CellsUnderThreat
    {

        // Finds cells under threat produced by piece at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // board: The board represents the current arrangement of all pieces.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by piece at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreat(Cell cell, IRectangularBoard board)
        {
            var piece = board.GetPiece(cell);

            // Return empty IEnumerable collection if cell is empty.
            if (piece == null)
            {
                return new List<Cell>();
            }

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: piece.Value.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: piece.Value.Color.Change());

            var mappingPieceTypeToMethod = new Dictionary<PieceType,
                                                          Func<Cell, IEnumerable<Cell>,
                                                          IEnumerable<Cell>,
                                                          Func<Cell, bool>,
                                                          Color,
                                                          IEnumerable<Cell>>>()
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
                                            board.IsOnBoard,
                                            (Color)piece?.Color);
        }

        // Finds cells under threat produced by piece at cell |cell|. 
        // This function is a helper one for other methods.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // shifts: A IEnumerable collection containing possible cell shifts.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // oneShift: Check only |shifts| if true, otherwise perform shifts until the cells are on board.
        // For example, |oneShift| is true for bishop, rook, and queen, while false for pawn, knight, and king.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by piece at cell |cell|. 
        private static IEnumerable<Cell> FindCells(Cell cell,
                                                   IEnumerable<Cell> shifts,
                                                   IEnumerable<Cell> pieceCells,
                                                   IEnumerable<Cell> enemyPieceCells,
                                                   Func<Cell, bool> IsOnBoard,
                                                   bool oneShift = false)
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
                    if (!IsOnBoard(currentCell)
                        || pieceCells.Contains(currentCell)) break;

                    cellsUnderThreat.Add(currentCell);

                    if (oneShift
                        || enemyPieceCells.Contains(currentCell)) break;
                }
            }
            return cellsUnderThreat;
        }

        // Finds cells under threat produced by rook at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The rook color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by rook at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatRook(Cell cell,
                                                                IEnumerable<Cell> pieceCells,
                                                                IEnumerable<Cell> enemyPieceCells,
                                                                Func<Cell, bool> IsOnBoard,
                                                                Color activeColor)
        {
            // up and down over y-axis or files
            // right and left - x-axis or ranks
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
                             IsOnBoard,
                             oneShift: false);
        }

        // Finds cells under threat produced by knight at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The knight color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by knight at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatKnight(Cell cell,
                                                                  IEnumerable<Cell> pieceCells,
                                                                  IEnumerable<Cell> enemyPieceCells,
                                                                  Func<Cell, bool> IsOnBoard,
                                                                  Color activeColor)
        {
            var xs = new[] { -2, -1, 1, 2 };
            var ys = new[] { -2, -1, 1, 2 };
            var shifts =
                (from x in xs
                 from y in ys
                 select new { x, y }) // Get all combinations.
                .Where((tuple) => Math.Abs(tuple.x) != Math.Abs(tuple.y))
                .Select((tuple) => new Cell(tuple.x, tuple.y));

            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             IsOnBoard,
                             oneShift: true);

        }

        // Finds cells under threat produced by bishop at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The bishop color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by bishop at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatBishop(Cell cell,
                                                                  IEnumerable<Cell> pieceCells,
                                                                  IEnumerable<Cell> enemyPieceCells,
                                                                  Func<Cell, bool> IsOnBoard,
                                                                  Color activeColor)

        {
            // up and down over cells in file.
            // right and left over cells in rank.
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
                             IsOnBoard,
                             oneShift: false);
        }

        // Finds cells under threat produced by queen at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The queen color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by queen at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatQueen(Cell cell,
                                                                 IEnumerable<Cell> pieceCells,
                                                                 IEnumerable<Cell> enemyPieceCells,
                                                                 Func<Cell, bool> IsOnBoard,
                                                                 Color activeColor)

        {
            var cellsUnderThreatRook = GetCellsUnderThreatRook(cell,
                                                               pieceCells,
                                                               enemyPieceCells,
                                                               IsOnBoard,
                                                               activeColor);

            var cellsUnderThreatBishop = GetCellsUnderThreatBishop(cell,
                                                                   pieceCells,
                                                                   enemyPieceCells,
                                                                   IsOnBoard,
                                                                   activeColor);
            // Concatenate/combine rook and bishop cells. 
            return cellsUnderThreatRook.Concat(cellsUnderThreatBishop);
        }

        // Finds cells under threat produced by king at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The king color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by king at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatKing(Cell cell,
                                                                IEnumerable<Cell> pieceCells,
                                                                IEnumerable<Cell> enemyPieceCells,
                                                                Func<Cell, bool> IsOnBoard,
                                                                Color activeColor)
        {
            var xs = new[] { -1, 0, 1 };
            var ys = new[] { -1, 0, 1 };
            var shifts =
                (from x in xs
                 from y in ys
                 select new { x, y }) // Get all combinations.
                .Where((tuple) => !(tuple.x == 0 && tuple.y == 0)) // Exclude tuple corresponding to |cell|.
                .Select((tuple) => new Cell(tuple.x, tuple.y));

            return FindCells(cell,
                             shifts,
                             pieceCells,
                             enemyPieceCells,
                             IsOnBoard,
                             oneShift: true);
        }

        // Finds cells under threat produced by pawn at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // pieceCells: A IEnumerable collection containing pieces that belong to the same color as the piece at cell |cell|.
        // enemyPieceCells: A IEnumerable collection containing pieces that belong to the enemy color.
        // IsOnBoard: A function to decide on where the cell is on board.
        // activeColor: The pawn color.
        //
        // Returns
        // -------
        // A IEnumerable collection containing cells under threat produced by pawn at cell |cell|.
        public static IEnumerable<Cell> GetCellsUnderThreatPawn(Cell cell,
                                                                IEnumerable<Cell> pieceCells,
                                                                IEnumerable<Cell> enemyPieceCells,
                                                                Func<Cell, bool> IsOnBoard,
                                                                Color activeColor)
        {
            var leftShift = activeColor == Color.White ? new Cell(-1, 1) : new Cell(-1, -1);
            var rightShift = activeColor == Color.White ? new Cell(1, 1) : new Cell(1, -1);

            // A list to save cells 'under threat'
            var cellsUnderThreat = new List<Cell>();

            // Iterate over shifts and find cells 'under threat'
            foreach (var shift in new Cell[] { leftShift, rightShift })
            {
                var currentCell = cell + shift;
                if (enemyPieceCells.Contains(currentCell))
                    cellsUnderThreat.Add(currentCell);
            }

            return cellsUnderThreat;
        }
    }
}
