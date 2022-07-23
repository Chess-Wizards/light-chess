using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine.UnderThreats
{
    // Finds a INumerable collection of cells 'under threat'. 'under threat' means all cells
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

            var pieceUnderThreatCells = _PieceUnderThreatCells(piece.Value.Type);

            return _FindCells(cell,
                              pieceUnderThreatCells.Shifts[piece.Value.Color],
                              pieceCells,
                              enemyPieceCells,
                              board.IsOnBoard,
                              pieceUnderThreatCells.IsOneShift);
        }

        private static IPieceUnderThreatCells _PieceUnderThreatCells(PieceType pieceType)
        {
                if (pieceType == PieceType.Rook)
                { 
                    return new RookUnderThreatCells();
                }
                else if (pieceType == PieceType.Knight)
                {
                    return new KnightUnderThreatCells();
                }
                else if (pieceType == PieceType.Bishop)
                {
                    return new BishopUnderThreatCells();
                }
                else if (pieceType == PieceType.Queen)
                {
                    return new QueenUnderThreatCells();
                }
                else if (pieceType == PieceType.King)
                {
                    return new KingUnderThreatCells();
                }
                else if(pieceType == PieceType.Pawn)
                {
                    return new PawnUnderThreatCells();
                }

                throw new ArgumentException("Invalid argument");
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
        private static IEnumerable<Cell> _FindCells(Cell cell,
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
    }
}
