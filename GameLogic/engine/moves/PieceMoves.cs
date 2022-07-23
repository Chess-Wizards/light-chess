using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine.Moves
{
    // The class aims to find the array of all moves. Moves cannot be valid.
    // This class does not consider checks, en passant moves, and castles.
    static public class PieceMoves
    {
        private static readonly PieceConstants _PieceConstants = new();

        // Finds moves produced by piece at cell |cell|.
        //
        // Parameters
        // ----------
        // cell: The cell.
        // board: The board represents the current arrangement of all pieces.
        //
        // Returns
        // -------
        // A IEnumerable collection containing moves produced by piece at cell |cell|.   
        public static IEnumerable<Move> GetMoves(Cell cell, IRectangularBoard board)
        {
            var piece = board.GetPiece(cell);

            // Return an empty IEnumerable collection if the cell is empty.
            if (piece == null)
            {
                return new List<Move>() { };
            }

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: piece.Value.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: (piece.Value.Color).Change());

            var cellsUnderThreat = CellsUnderThreat.GetCellsUnderThreat(cell, board);

            if (piece.Value.Type == PieceType.Pawn)
            {
                cellsUnderThreat = _GetNextCellsPawn(
                                                    cell,
                                                    cellsUnderThreat,
                                                    pieceCells,
                                                    enemyPieceCells.ToList(),
                                                    piece.Value.Color
                                                    );
            }

            return cellsUnderThreat
                // Suggest four moves, if the pawn promotion is applied. Otherwise, only move is suggested.
                .SelectMany(nextCell => (piece.Value.Type == PieceType.Pawn && _IsPawnPromotionRank(nextCell.Y)
                                        ? _PieceConstants.possiblePromotionPieceTypes.Select(pieceType => new Move(cell, nextCell, pieceType))
                                        : new List<Move>() { new Move(cell, nextCell) }));
        }
        
        private static bool _IsPawnPromotionRank(int rank)
        {
            return _PieceConstants.BlackPawnPromotionRank == rank || _PieceConstants.WhitePawnPromotionRank == rank;
        }

        // Finds next/move cells produced by pawn at cell |cell|.
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
        // A IEnumerable collection containing next/move cells produced by pawn at cell |cell|.
        private static IEnumerable<Cell> _GetNextCellsPawn(Cell cell,
                                                           IEnumerable<Cell> cellsUnderThreat,
                                                           IEnumerable<Cell> pieceCells,                                                           
                                                           IList<Cell> enemyPieceCells,
                                                           Color activeColor)
        {
            var nextCells = cellsUnderThreat.Where(cell => enemyPieceCells.Contains(cell))
                                            .ToList();
                                                         
            // Shift depends on color.            
            var shift = activeColor == Color.White ? new Cell(0, 1) : new Cell(0, -1);

            var cellOneMoveForward = cell + shift;
            // One move forward.
            if (!pieceCells.Contains(cellOneMoveForward)
                && !enemyPieceCells.Contains(cellOneMoveForward))
            {
                nextCells.Add(cellOneMoveForward);
            }

            // Two moves forward.
            var cellTwoMovesForward = cellOneMoveForward + shift;
            var initialPawnRank = activeColor == Color.White ? _PieceConstants.WhiteInitialPawnRank : _PieceConstants.BlackInitialPawnRank;
            var notTouchedPawn = cell.Y == initialPawnRank;
            if (notTouchedPawn
                && !pieceCells.Contains(cellOneMoveForward)
                && !enemyPieceCells.Contains(cellTwoMovesForward))
            {
                nextCells.Add(cellTwoMovesForward);
            }

            return nextCells;
        }
    }
}
