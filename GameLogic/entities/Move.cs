using GameLogic.Entities.Pieces;

namespace GameLogic.Entities
{
    // Defines a move.
    // Each move can be uniquely identified by a triplet: {start cell, end cell, final piece type}.
    // For example, the pawn can be promoted to another piece. Therefore, the piece type is required.
    public struct Move
    {
        public Cell StartCell { get; }

        public Cell EndCell { get; }

        public PieceType? PromotedPieceType { get; }

        public Move(Cell startCell, Cell endCell, PieceType? promotedPieceType = null)
        {
            StartCell = startCell;
            EndCell = endCell;
            PromotedPieceType = promotedPieceType;
        }

        public static bool operator ==(Move move1, Move move2)
        {
            return move1.Equals(move2);
        }

        public static bool operator !=(Move move1, Move move2)
        {
            return !move1.Equals(move2);
        }

        public override bool Equals(object obj) // TODO: check CS8765
        {
            if (obj == null)
            {
                return false;
            }

            var move = (Move)obj;

            return StartCell == move.StartCell &&
                EndCell == move.EndCell &&
                PromotedPieceType == move.PromotedPieceType;
        }

        public override int GetHashCode()
        {
            return StartCell.GetHashCode() ^
                EndCell.GetHashCode() ^
                (PromotedPieceType == null ? 0 : PromotedPieceType).GetHashCode();
        }
    }
}
