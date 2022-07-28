namespace GameLogic.Entities.Pieces
{
    class PieceConstants
    {
        public int WhiteInitialPawnRank { get { return Y._1; } }

        public int WhitePawnPromotionRank { get { return Y._7; } }

        public int BlackInitialPawnRank { get { return Y._6; } }

        public int BlackPawnPromotionRank { get { return Y._0; } }

        public IList<int> InvalidPawnRanks { get { return new List<int> { Y._0, Y._7 }; } }
        public int MaxForwardPawnMovesNotTouched { get { return 2 * Y.Unit; } }

        public int MaxForwardPawnMovesTouched { get { return Y.Unit; } }

        public IList<PieceType> PossiblePromotionPieceTypes
        {
            get
            {
                return new List<PieceType>{PieceType.Knight,
                                                                                   PieceType.Bishop,
                                                                                   PieceType.Rook,
                                                                                   PieceType.Queen};
            }
        }

        public IDictionary<Color, IList<Cell>> ShiftsForEnPassantMove = new Dictionary<Color, IList<Cell>>(){
            {Color.White, new List<Cell>(){new Cell(X.Unit, Y.Unit), new Cell(-X.Unit, Y.Unit)}},
            {Color.Black, new List<Cell>(){new Cell(X.Unit, -Y.Unit), new Cell(-X.Unit, -Y.Unit)}}
        };


        public IDictionary<Color, Cell> NewEnPassantCellAfterMove = new Dictionary<Color, Cell>(){
            {Color.White, new Cell(X.Zero, -Y.Unit)},
            {Color.Black, new Cell(X.Zero, Y.Unit)}
        };
    }
}
