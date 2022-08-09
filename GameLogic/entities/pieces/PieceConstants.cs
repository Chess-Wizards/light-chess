namespace GameLogic.Entities.Pieces
{
    // this is also smth like StandardGamePieceConstants
    static class PieceConstants
    {
        public const int WhiteInitialPawnRank = Y._1;

        public const int WhitePawnPromotionRank = Y._7;

        public const int BlackInitialPawnRank = Y._6;

        public const int BlackPawnPromotionRank = Y._0;

        // why is smth "const int" and smth "static readonly" ?

        // maybe ISet ?
        public static readonly IList<int> InvalidPawnRanks = new List<int> { Y._0, Y._7 };

        // TODO: check but seems weird
        public const int MaxForwardPawnMovesNotTouched = 2 * Y.Unit;
        public const int MaxForwardPawnMovesTouched = Y.Unit;

        // maybe ISet ?
        public static readonly IList<PieceType> PossiblePromotionPieceTypes = new List<PieceType>{PieceType.Knight, PieceType.Bishop,
                                                                                         PieceType.Rook, PieceType.Queen};

        // TODO: check
        public static readonly IDictionary<Color, IList<Cell>> ShiftsForEnPassantMove = new Dictionary<Color, IList<Cell>>(){
            {Color.White, new List<Cell>(){new Cell(X.Unit, Y.Unit), new Cell(-X.Unit, Y.Unit)}},
            {Color.Black, new List<Cell>(){new Cell(X.Unit, -Y.Unit), new Cell(-X.Unit, -Y.Unit)}}
        };

        public static readonly IDictionary<Color, Cell> NewEnPassantCellAfterMove = new Dictionary<Color, Cell>(){
            {Color.White, new Cell(X.Zero, -Y.Unit)},
            {Color.Black, new Cell(X.Zero, Y.Unit)}
        };
    }
}
