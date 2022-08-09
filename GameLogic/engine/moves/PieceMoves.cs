using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Engine.Moves
{
    // The class aims to find the array of all moves. Moves cannot be valid. // TODO: Dear @SherlockKA, please look at the last sentence of this line.
    // This class does not consider checks, en passant moves, and castles.
    public static class PieceMoves
    {
        // Finds moves produced by piece at cell |cell|.
        public static IEnumerable<Move> GetMoves(Cell cell, IRectangularBoard board)
        {
            var piece = board.GetPiece(cell);

            // Return an empty IEnumerable collection if the cell is empty.
            if (piece == null)
            {
                return Enumerable.Empty<Move>();
            }

            // Divide pieces into own and enemy.
            var pieceCells = board.GetCellsWithPieces(filterByColor: piece.Value.Color);
            var enemyPieceCells = board.GetCellsWithPieces(filterByColor: (piece.Value.Color).Inversed());


            return _PieceCells(cell, piece.Value).SelectMany(option => option.GetCells(cell,
                                                                                       piece.Value.Color,
                                                                                       pieceCells,
                                                                                       enemyPieceCells,
                                                                                       board.IsOnBoard)
                                                           )
                                                 // Suggest four moves, if the pawn promotion is applied. Otherwise, only move is suggested.
                                                 .SelectMany(nextCell => (piece.Value.Type == PieceType.Pawn && _IsPawnPromotionRank(nextCell.Y)
                                                                        ? PieceConstants.PossiblePromotionPieceTypes.Select(pieceType => new Move(cell, nextCell, pieceType))
                                                                        : new List<Move>() { new Move(cell, nextCell) }));
        }

        private static bool _IsPawnPromotionRank(int rank)
        {
            return PieceConstants.BlackPawnPromotionRank == rank || PieceConstants.WhitePawnPromotionRank == rank;
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
                var shiftsNumber = _PawnIsNotTouched(cell.Y, piece.Color) ? PieceConstants.MaxForwardPawnMovesNotTouched : PieceConstants.MaxForwardPawnMovesTouched;
                return new List<IPieceCells>() { new PawnCells(shiftsNumber), new PawnCellsCapture() };
            }

            throw new ArgumentException("Invalid argument");
        }

        private static bool _PawnIsNotTouched(int rank, Color color)
        {
            return (color == Color.White ? PieceConstants.WhiteInitialPawnRank : PieceConstants.BlackInitialPawnRank) == rank;
        }
    }
}
