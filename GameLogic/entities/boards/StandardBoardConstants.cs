using GameLogic.Entities;

class StandardBoardConstants
{
    // First and last ranks define the direction of the pawn movement.
    public int Size { get { return 8 * Y.Unit; } }

    public int FirstWhiteRank { get { return Y._0; } }
    public int LastWhiteRank { get { return Y._7; } }
    public int FirstBlackRank { get { return Y._7; } }
    public int LastBlackRank { get { return Y._0; } }
}
