
namespace GameLogic.Entities.Castles
{
    class BlackQueenCastleConstants : ICastleConstant
    {
        public Cell InitialKingCell { get; } = new Cell(4, 7);
        public Cell InitialRookCell { get; } = new Cell(0, 7);
        public Cell FinalKingCell { get; } = new Cell(2, 7);
        public Cell FinalRookCell { get; } = new Cell(3, 7);
        public IList<Cell> RequiredEmptyCells { get; } = new List<Cell>() {
                                                            new Cell(1, 7), new Cell(2, 7), new Cell(3, 7)
                                                         };
    }
}
