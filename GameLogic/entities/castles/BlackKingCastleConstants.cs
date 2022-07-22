
namespace GameLogic.Entities.Castles
{
    class BlackKingCastleConstants : ICastleConstant
    {
        public Cell InitialKingCell { get; } = new Cell(4, 7);
        public Cell InitialRookCell { get; } = new Cell(7, 7);
        public Cell FinalKingCell { get; } = new Cell(6, 7);
        public Cell FinalRookCell { get; } = new Cell(5, 7);
        public IList<Cell> RequiredEmptyCells { get; } = new List<Cell>() {
                                                            new Cell(5, 7), new Cell(6, 7)
                                                         };
    }
}
