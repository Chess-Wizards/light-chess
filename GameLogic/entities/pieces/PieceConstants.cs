namespace GameLogic.Entities.Pieces
{
    class PieceConstants
    {
        public int WhiteInitialPawnRank { get; } = 1;

        public int WhitePawnPromotionRank { get; } = 7;

        public int BlackInitialPawnRank { get; } = 6;

        public int BlackPawnPromotionRank { get; } = 0;

        public IList<int> InvalidPawnRanks { get; } = new List<int> { 0, 7 };

        public int ForwardPawnMovesNotTouched { get; } = 2;

        public int ForwardPawnMovesTouched { get; } = 1;

        public IList<PieceType> possiblePromotionPieceTypes { get; } = new List<PieceType>{PieceType.Knight,
                                                                                           PieceType.Bishop,
                                                                                           PieceType.Rook,
                                                                                           PieceType.Queen};

        public IDictionary<Color, IList<Cell>> ShiftsForEnPassantMove = new Dictionary<Color, IList<Cell>>(){
            {Color.White, new List<Cell>(){new Cell(1, 1), new Cell(-1, 1)}},
            {Color.Black, new List<Cell>(){new Cell(1, -1), new Cell(-1, -1)}}
        };


        public IDictionary<Color, Cell> NewEnPassantCellAfterMove = new Dictionary<Color, Cell>(){
            {Color.White, new Cell(0, -1)},
            {Color.Black, new Cell(0, 1)}
        };
    }
}
