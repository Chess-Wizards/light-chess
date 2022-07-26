using GameLogic.Entities;

namespace GameLogic.Entities.Castles
{
    public interface ICastleTypeConstants
    {
        static int WhiteCastleRank { get { return Y._0; } }

        static int BlackCastleRank { get { return Y._7; } }

        Cell InitialKingCell { get; }

        Cell InitialRookCell { get; }

        Cell FinalKingCell { get; }

        Cell FinalRookCell { get; }

        IList<Cell> RequiredEmptyCells { get; }

        Move CastleMove { get { return new Move(InitialKingCell, FinalKingCell); } }
    }
}
