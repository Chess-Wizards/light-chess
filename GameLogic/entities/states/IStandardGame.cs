using GameLogic.Entities.Boards;

namespace GameLogic.Entities.States
{
    public interface IGameState<out TBoard> where TBoard : IBoard
    {
        TBoard Board { get; }
        Color ActiveColor { get; }
    }
}
