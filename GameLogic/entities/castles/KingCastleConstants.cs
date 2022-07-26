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
            InitialKingCell = new Cell(4, _initialRank);
            InitialRookCell = new Cell(7, _initialRank);
            FinalKingCell = new Cell(6, _initialRank);
            FinalRookCell = new Cell(5, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(5, _initialRank), new Cell(6, _initialRank) };
        }
    }
}
