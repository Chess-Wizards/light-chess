using GameLogic.Entities.Boards;
using GameLogic.Entities.Castlings;

namespace GameLogic.Entities.States
{
    // Represents standard chess game functionality. 
    public interface IStandardGameState : IGameState<IRectangularBoard>
    {
        IEnumerable<Castling> AvailableCastlings { get; }
        Cell? EnPassantCell { get; }
        int HalfmoveNumber { get; }
        int FullmoveNumber { get; }
        // TODO: do replace with ActiveColor?
        Color EnemyColor { get; }
    }
}
