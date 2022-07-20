using GameLogic.Entities.Boards;

namespace GameLogic.Entities.States
{
    // Represents state functionality. 
    public interface IStandardGameState
    {
        IRectangularBoard Board { get; }
        IEnumerable<Castle> AvailableCastles { get; }
        Cell? EnPassantCell { get; }
        int HalfmoveNumber { get; }
        int FullmoveNumber { get; }
        Color ActiveColor { get; }
        Color EnemyColor { get; }
    }
}
