using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine.Moves
{
    // Finds a INumerable collection of cells 'under threat'. 'under threat' means all cells
    // at which the enemy king cannot stand because of the check. In addition, the piece must be able to 
    // make a move at this cell. For example, a pawn can capture iff the enemy piece stands at a diagonal.
    //
    // This class does not consider checks, en passant moves, and castles.
    static public class CellsUnderThreat
    {

        // Finds cells under threat produced by piece at cell |cell|.
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

            return pieceUnderThreatCells.GetCells(cell,
                                                  piece.Value.Color,
                                                  pieceCells,
                                                  enemyPieceCells,
                                                  board.IsOnBoard);
        }

        private static IPieceCells _PieceUnderThreatCells(PieceType pieceType)
        {
            if (pieceType == PieceType.Rook)
            {
                return new RookCells();
            }
            else if (pieceType == PieceType.Knight)
            {
                return new KnightCells();
            }
            else if (pieceType == PieceType.Bishop)
            {
                return new BishopCells();
            }
            else if (pieceType == PieceType.Queen)
            {
                return new QueenCells();
            }
            else if (pieceType == PieceType.King)
            {
                return new KingCells();
            }
            else if (pieceType == PieceType.Pawn)
            {
                return new PawnCellsUnderThreat();
            }

            throw new ArgumentException("Invalid argument");
        }
    }
}
