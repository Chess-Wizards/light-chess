namespace GameLogic.Entities.Pieces
{
    public interface IPieceShiftConstants
    {
        IEnumerable<Cell> Shifts { get; }
        bool IsOneShift { get; }
    }
}
