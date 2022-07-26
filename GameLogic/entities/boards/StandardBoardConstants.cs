using GameLogic.Entities;

class StandardBoardConstants
{
    // First and last ranks define the direction of the pawn movement.
    public int Size => 8 * Y.Unit;

    public int FirstWhiteRank => Y._0;
    public int LastWhiteRank => Y._7;
    public int FirstBlackRank => Y._7;
    public int LastBlackRank => Y._0;
}
