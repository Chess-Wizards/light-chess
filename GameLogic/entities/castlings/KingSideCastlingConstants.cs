namespace GameLogic.Entities.Castlings
{
    class KingSideCastlingConstants : ICastlingTypeConstants
    {
        public Cell InitialKingCell { get; }
        public Cell InitialRookCell { get; }
        public Cell FinalKingCell { get; }
        public Cell FinalRookCell { get; }

        public IList<Cell> RequiredEmptyCells { get; }

        public KingSideCastlingConstants(Color color)
        {
            var _initialRank = (color == Color.White)
                ? ICastlingTypeConstants.WhiteCastleRank
                : ICastlingTypeConstants.BlackCastleRank;

            InitialKingCell = new Cell(X.E, _initialRank);
            InitialRookCell = new Cell(X.H, _initialRank);
            FinalKingCell = new Cell(X.G, _initialRank);
            FinalRookCell = new Cell(X.F, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(X.F, _initialRank),
                new Cell(X.G, _initialRank) };
        }
    }
}
