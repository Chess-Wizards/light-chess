namespace GameLogic.Entities.Castles
{
    public interface ICastleTypeConstants
    {
        Cell InitialKingCell { get; }
        Cell InitialRookCell { get; }
        Cell FinalKingCell { get; }
        Cell FinalRookCell { get; }
        IList<Cell> RequiredEmptyCells { get; }
        Move GetCastleMove { get { return new Move(InitialKingCell, FinalKingCell); } }

    }
}
