using GameLogic.Entities;

namespace GameLogic.Engine.UnderThreats
{
    public interface IPieceUnderThreatCells
    {
        IDictionary<Color, IEnumerable<Cell>> Shifts { get; }
        bool IsOneShift { get; }
    }
}
