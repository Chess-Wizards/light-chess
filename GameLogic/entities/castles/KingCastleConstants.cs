namespace GameLogic.Entities.Castles
{
    class KingCastleConstants : ICastleTypeConstants
    {
        public Cell InitialKingCell { get; }

        public Cell InitialRookCell { get; }

        public Cell FinalKingCell { get; }

        public Cell FinalRookCell { get; }

        public IList<Cell> RequiredEmptyCells { get; }

        public KingCastleConstants(Color color)
        {
            var _initialRank = color == Color.White ? ICastleTypeConstants.WhiteCastleRank : ICastleTypeConstants.BlackCastleRank;
            InitialKingCell = new Cell(X.E, _initialRank);
            InitialRookCell = new Cell(X.H, _initialRank);
            FinalKingCell = new Cell(X.G, _initialRank);
            FinalRookCell = new Cell(X.F, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(X.F, _initialRank), new Cell(X.G, _initialRank) };
        }
    }
}
