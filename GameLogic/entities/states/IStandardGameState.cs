using GameLogic.Entities.Boards;

namespace GameLogic.Entities.States
{
    // Represents state functionality. 
    public interface IStandardGameState: IGameState<IRectangularBoard>
    {
        IEnumerable<Castle> AvailableCastles { get; }
        Cell? EnPassantCell { get; }
        int HalfmoveNumber { get; }
        int FullmoveNumber { get; }
        Color EnemyColor { get; }
    }
}
