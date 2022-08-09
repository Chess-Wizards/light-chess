using GameLogic.Entities;

class StandardBoardConstants
{
    public const int Size = 8 * Y.Unit;

    // First and last ranks define the direction of the pawn movement.
    public const int FirstWhiteRank = Y._0;
    public const int LastWhiteRank = Y._7;
    public const int FirstBlackRank = Y._7;
    public const int LastBlackRank = Y._0;
}
