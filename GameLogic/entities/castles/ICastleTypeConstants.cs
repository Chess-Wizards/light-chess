using GameLogic.Entities;

namespace GameLogic.Entities.Castles
{
    public interface ICastleTypeConstants
    {
        static int WhiteCastleRank { get; } = Y._0;

        static int BlackCastleRank { get; } = Y._7;
        Move CastleMove => new Move(InitialKingCell, FinalKingCell);

        Cell InitialKingCell { get; }

        Cell InitialRookCell { get; }

        Cell FinalKingCell { get; }

        Cell FinalRookCell { get; }

        IList<Cell> RequiredEmptyCells { get; }
    }
}
