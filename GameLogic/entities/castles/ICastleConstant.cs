namespace GameLogic.Entities.Castles
{
    public interface ICastleConstant
    {
        Cell InitialKingCell { get; }
        Cell InitialRookCell { get; }
        Cell FinalKingCell { get; }
        Cell FinalRookCell { get; }
        IList<Cell> RequiredEmptyCells { get; }

    }
}
