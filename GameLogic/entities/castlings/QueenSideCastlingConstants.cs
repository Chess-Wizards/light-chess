namespace GameLogic.Entities.Castlings
{
    class QueenSideCastlingConstants : ICastlingTypeConstants
    {
        public Cell InitialKingCell { get; }
        public Cell InitialRookCell { get; }
        public Cell FinalKingCell { get; }
        public Cell FinalRookCell { get; }

        public IList<Cell> RequiredEmptyCells { get; }
        public QueenSideCastlingConstants(Color color)
        {
            var _initialRank = (color == Color.White)
                ? ICastlingTypeConstants.WhiteCastleRank
                : ICastlingTypeConstants.BlackCastleRank;

            InitialKingCell = new Cell(X.E, _initialRank);
            InitialRookCell = new Cell(X.A, _initialRank);
            FinalKingCell = new Cell(X.C, _initialRank);
            FinalRookCell = new Cell(X.D, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(X.B, _initialRank),
                new Cell(X.C, _initialRank), new Cell(X.D, _initialRank) };
        }
    }
}
