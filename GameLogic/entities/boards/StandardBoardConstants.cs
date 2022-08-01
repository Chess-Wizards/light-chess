using GameLogic.Entities;

class StandardBoardConstants
{
    // First and last ranks define the direction of the pawn movement.
    public int Size { get; } = 8 * Y.Unit;
    public int FirstWhiteRank { get; } = Y._0;
    public int LastWhiteRank { get; } = Y._7;
    public int FirstBlackRank { get; }= Y._7;
    public int LastBlackRank { get; } = Y._0;
}
