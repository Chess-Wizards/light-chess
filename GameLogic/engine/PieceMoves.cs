using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine
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

            var possiblePromotionPieceTypes = new List<PieceType>{PieceType.Knight,
                                                                  PieceType.Bishop,
                                                                  PieceType.Rook,
                                                                  PieceType.Queen};

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: piece.Value.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: (piece.Value.Color).Change());

            var mappingPieceTypeToMethod = new Dictionary<PieceType,
                                                          Func<Cell, IEnumerable<Cell>,
                                                          IEnumerable<Cell>,
                                                          Func<Cell, bool>,
                                                          Color,
                                                          IEnumerable<Cell>>>()
            {
                {PieceType.Rook, CellsUnderThreat.GetCellsUnderThreatRook},
                {PieceType.Knight, CellsUnderThreat.GetCellsUnderThreatKnight},
                {PieceType.Bishop, CellsUnderThreat.GetCellsUnderThreatBishop},
                {PieceType.Queen, CellsUnderThreat.GetCellsUnderThreatQueen},
                {PieceType.King, CellsUnderThreat.GetCellsUnderThreatKing},
                {PieceType.Pawn, _GetNextCellsPawn}
            };

            return mappingPieceTypeToMethod[piece.Value.Type](cell,
                                            pieceCells,
                                            enemyPieceCells,
                                            board.IsOnBoard,
                                            piece.Value.Color)
                // Suggest four moves, if the pawn promotion is applied. Otherwise, only move os suggested.
                .SelectMany(nextCell => (piece.Value.Type == PieceType.Pawn  && _IsPawnPromotionRank(nextCell.Y)
                                        ? possiblePromotionPieceTypes.Select(pieceType => new Move(cell, nextCell, pieceType))
                                        : new List<Move>() { new Move(cell, nextCell) }));
        }
        private static bool _IsPawnPromotionRank(int rank)
        {
            return _PieceConstants.BlackPawnPromotionRank == rank|| _PieceConstants.WhitePawnPromotionRank == rank;
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
                                                           IEnumerable<Cell> pieceCells,
                                                           IEnumerable<Cell> enemyPieceCells,
                                                           Func<Cell, bool> IsOnBoard,
                                                           Color activeColor)
        {
            var cellsNext = CellsUnderThreat.GetCellsUnderThreatPawn(cell,
                                                                     pieceCells,
                                                                     enemyPieceCells,
                                                                     IsOnBoard,
                                                                     activeColor).ToList();

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
            var cellTwoMovesForward = cellOneMoveForward + shift;
            var initialPawnRank = activeColor == Color.White ? _PieceConstants.WhiteInitialPawnRank : _PieceConstants.BlackInitialPawnRank;
            var notTouchedPawn = cell.Y == initialPawnRank;
            if (notTouchedPawn
                && !pieceCells.Contains(cellOneMoveForward)
                && !enemyPieceCells.Contains(cellTwoMovesForward))
            {
                cellsNext.Add(cellTwoMovesForward);
            }

            return cellsNext;
        }
    }
}
