using GameLogic.Entities.Pieces;

namespace GameLogic.Entities
{
    // Defines a move. Each move can
    // be uniquely identified by a triplet of start cell, end cell, and end piece type.
    // For example, the pawn can be promoted to another piece. Therefore, the piece type is required.
    public struct Move
    {
        public Cell StartCell { get; }

        public Cell EndCell { get; }

        public PieceType? PromotionPieceType { get; }

        public Move(Cell startCell, Cell endCell, PieceType? promotionPieceType = null)
        {
            StartCell = startCell;
            EndCell = endCell;
            PromotionPieceType = promotionPieceType;
        }

        public static bool operator ==(Move move1, Move move2)
        {
            return move1.Equals(move2);
        }

        public static bool operator !=(Move move1, Move move2)
        {
            return !move1.Equals(move2);
        }

        public override bool Equals(object obj)
        {
            var move = (Move)obj;
            if (move == null)
            {
                return false;
            }

            return StartCell == move.StartCell && EndCell == move.EndCell && PromotionPieceType == move.PromotionPieceType;
        }

        public override int GetHashCode()
        {
            return StartCell.GetHashCode() ^ EndCell.GetHashCode() ^ (PromotionPieceType == null ? 0 : PromotionPieceType).GetHashCode();
        }
    }
}
