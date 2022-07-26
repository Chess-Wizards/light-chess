
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
            var _initialRank = color == Color.White ? ICastleTypeConstants.WhiteCastleRank : ICastleTypeConstants.BlackCastleRank;
            InitialKingCell = new Cell(4, _initialRank);
            InitialRookCell = new Cell(0, _initialRank);
            FinalKingCell = new Cell(2, _initialRank);
            FinalRookCell = new Cell(3, _initialRank);
            RequiredEmptyCells = new List<Cell>() { new Cell(1, _initialRank), new Cell(2, _initialRank), new Cell(3, _initialRank) };
        }
    }
}
