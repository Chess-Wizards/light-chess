namespace GameLogic.Entities.Pieces
{
    class PieceConstants
    {
        public int WhiteInitialPawnRank { get; } = 1;
        public int WhitePawnPromotionRank { get; } = 7;
        public int BlackInitialPawnRank { get; } = 6;
        public int BlackPawnPromotionRank { get; } = 0;
        public IList<int> InvalidPawnRanks { get; } = new List<int> { 0, 7 };

        public IList<PieceType> possiblePromotionPieceTypes { get; } = new List<PieceType>{PieceType.Knight,
                                                                                           PieceType.Bishop,
                                                                                           PieceType.Rook,
                                                                                           PieceType.Queen};        
    }
}
