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
    }
}
