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


            return _PieceCells(cell, piece.Value).SelectMany(option => option.GetCells(cell,
                                                                                       piece.Value.Color,
                                                                                       pieceCells,
                                                                                       enemyPieceCells.ToList(),
                                                                                       board.IsOnBoard)
                                                           )
                                                 // Suggest four moves, if the pawn promotion is applied. Otherwise, only move is suggested.
                                                 .SelectMany(nextCell => (piece.Value.Type == PieceType.Pawn && _IsPawnPromotionRank(nextCell.Y)
                                                                        ? _PieceConstants.possiblePromotionPieceTypes.Select(pieceType => new Move(cell, nextCell, pieceType))
                                                                        : new List<Move>() { new Move(cell, nextCell) }));
        }

        private static bool _IsPawnPromotionRank(int rank)
        {
            return _PieceConstants.BlackPawnPromotionRank == rank || _PieceConstants.WhitePawnPromotionRank == rank;
        }

        private static IEnumerable<IPieceCells> _PieceCells(Cell cell, Piece piece)
        {
            if (piece.Type == PieceType.Rook)
            {
                return new List<IPieceCells>() { new RookCells() };
            }
            else if (piece.Type == PieceType.Knight)
            {
                return new List<IPieceCells>() { new KnightCells() };
            }
            else if (piece.Type == PieceType.Bishop)
            {
                return new List<IPieceCells>() { new BishopCells() };
            }
            else if (piece.Type == PieceType.Queen)
            {
                return new List<IPieceCells>() { new QueenCells() };
            }
            else if (piece.Type == PieceType.King)
            {
                return new List<IPieceCells>() { new KingCells() };
            }
            else if (piece.Type == PieceType.Pawn)
            {
                var numberShifts = _PawnIsNotTouched(cell.Y, piece.Color) ? _PieceConstants.ForwardPawnMovesNotTouched : _PieceConstants.ForwardPawnMovesTouched;
                return new List<IPieceCells>() { new PawnCells(numberShifts), new PawnCellsCapture() };
            }

            throw new ArgumentException("Invalid argument");
        }

        private static bool _PawnIsNotTouched(int rank, Color color)
        {
            return (color == Color.White ? _PieceConstants.WhiteInitialPawnRank : _PieceConstants.BlackInitialPawnRank) == rank;
        }
    }
}
