
namespace GameLogic.Entities.Castles
{
    class WhiteQueenCastleConstants : ICastleTypeConstants
    {
        public Cell InitialKingCell { get; } = new Cell(4, 0);

        public Cell InitialRookCell { get; } = new Cell(0, 0);

        public Cell FinalKingCell { get; } = new Cell(2, 0);

        public Cell FinalRookCell { get; } = new Cell(3, 0);

        public IList<Cell> RequiredEmptyCells { get; } = new List<Cell>() {
                                                            new Cell(1, 0), new Cell(2, 0), new Cell(3, 0)
                                                         };
    }
}
