namespace GameLogic.Entities.Castles
{
    public interface ICastleTypeConstants
    {
        // if using these rank values then it should be smth like IStandardCastleTypeConstants interface
        // I don't like that name of the interface
        static int WhiteCastleRank { get; } = Y._0;
        static int BlackCastleRank { get; } = Y._7;
        Move CastleMove => new(InitialKingCell, FinalKingCell);

        Cell InitialKingCell { get; }

        Cell InitialRookCell { get; }

        Cell FinalKingCell { get; }

        Cell FinalRookCell { get; }

        IList<Cell> RequiredEmptyCells { get; } // maybe ISet, not IList ?
    }
}

// all that castling-related logic seems quite complicated. TODO: check again
