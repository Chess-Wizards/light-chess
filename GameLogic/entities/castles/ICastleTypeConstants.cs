namespace GameLogic.Entities.Castles
{
    public interface ICastleTypeConstants
    {
        // public int WhiteCastleRank { get { return 0; } }

        const int WhiteCastleRank = 0;

        const int BlackCastleRank = 7;

        Cell InitialKingCell { get; }

        Cell InitialRookCell { get; }

        Cell FinalKingCell { get; }

        Cell FinalRookCell { get; }

        IList<Cell> RequiredEmptyCells { get; }

        Move GetCastleMove { get { return new Move(InitialKingCell, FinalKingCell); } }

    }
}
