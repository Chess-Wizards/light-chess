namespace GameLogic.Entities.Castlings
{
    public interface ICastlingTypeConstants
    {
        // TODO: if using these rank values then it should be smth like IStandardCastlingTypeConstants interface?
        static int WhiteCastleRank { get; } = Y._0;
        static int BlackCastleRank { get; } = Y._7;
        Move CastlingMove => new(InitialKingCell, FinalKingCell);

        Cell InitialKingCell { get; }
        Cell InitialRookCell { get; }
        Cell FinalKingCell { get; }
        Cell FinalRookCell { get; }

        IList<Cell> RequiredEmptyCells { get; }
    }
}
