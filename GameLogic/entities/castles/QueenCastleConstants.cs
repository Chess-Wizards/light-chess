namespace GameLogic.Entities.Castles
{
    class QueenCastleConstants : ICastleTypeConstants
    {
        public Cell InitialKingCell { get; }

        public Cell InitialRookCell { get; }

        public Cell FinalKingCell { get; }

        public Cell FinalRookCell { get; }

        public IList<Cell> RequiredEmptyCells { get; }
        public QueenCastleConstants(Color color)
        {
            var _initialRank = (color == Color.White)
                ? ICastleTypeConstants.WhiteCastleRank
                : ICastleTypeConstants.BlackCastleRank;

            InitialKingCell = new Cell(X.E, _initialRank);
            InitialRookCell = new Cell(X.A, _initialRank);
            FinalKingCell = new Cell(X.C, _initialRank);
            FinalRookCell = new Cell(X.D, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(X.B, _initialRank), new Cell(X.C, _initialRank), new Cell(X.D, _initialRank) };
        }
    }
}
