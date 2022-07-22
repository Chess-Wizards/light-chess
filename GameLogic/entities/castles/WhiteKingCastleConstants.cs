
namespace GameLogic.Entities.Castles
{
    class WhiteKingCastleConstants : ICastleConstant
    {
        public Cell InitialKingCell { get; } = new Cell(4, 0);
        public Cell InitialRookCell { get; } = new Cell(7, 0);
        public Cell FinalKingCell { get; } = new Cell(6, 0);
        public Cell FinalRookCell { get; } = new Cell(5, 0);
        public IList<Cell> RequiredEmptyCells { get; } = new List<Cell>() {
                                                            new Cell(5, 0), new Cell(6, 0)
                                                         };
    }
}
