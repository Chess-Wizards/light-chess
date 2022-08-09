namespace GameLogic.Entities.Pieces
{
    // TODO: maybe to rename to smth like StandardGamePieceConstants? In according to the initial and promotion rank values.
    static class PieceConstants
    {
        public const int WhiteInitialPawnRank = Y._1;
        public const int WhitePawnPromotionRank = Y._7;

        public const int BlackInitialPawnRank = Y._6;
        public const int BlackPawnPromotionRank = Y._0;

        public const int MaxForwardPawnMovesNotTouched = 2 * Y.Unit;
        public const int MaxForwardPawnMovesTouched = Y.Unit;

        public static readonly ISet<int> InvalidPawnRanks = new HashSet<int> { Y._0, Y._7 };

        public static readonly ISet<PieceType> PossiblePromotionPieceTypes =
            new HashSet<PieceType>{PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Queen};

        public static readonly IDictionary<Color, IList<Cell>> ShiftsForEnPassantMove = new Dictionary<Color, IList<Cell>>()
        {
            {Color.White, new List<Cell>(){new Cell(X.Unit, Y.Unit), new Cell(-X.Unit, Y.Unit)}},
            {Color.Black, new List<Cell>(){new Cell(X.Unit, -Y.Unit), new Cell(-X.Unit, -Y.Unit)}}
        };

        public static readonly IDictionary<Color, Cell> NewEnPassantCellAfterMove = new Dictionary<Color, Cell>()
        {
            {Color.White, new Cell(X.Zero, -Y.Unit)},
            {Color.Black, new Cell(X.Zero, Y.Unit)}
        };
    }
}
