using GameLogic.Entities.Boards;

namespace GameLogic.Entities.States
{
    public interface IGameState<TBoard>
    {
        TBoard Board { get; }
        Color ActiveColor { get; }
    }
}
