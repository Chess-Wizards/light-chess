using GameLogic.Entities.Boards;
using GameLogic.Entities.Castles;

namespace GameLogic.Entities.States
{
    // Represents standard state functionality. 
    public interface IStandardGameState : IGameState<IRectangularBoard>
    {
        IEnumerable<Castle> AvailableCastles { get; }
        Cell? EnPassantCell { get; }
        int HalfmoveNumber { get; }
        int FullmoveNumber { get; }
        Color EnemyColor { get; }
    }
}
