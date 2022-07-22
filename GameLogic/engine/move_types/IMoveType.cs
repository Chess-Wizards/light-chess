using GameLogic.Entities.Boards;
using GameLogic.Entities;

namespace GameLogic.Engine.MoveTypes
{
    public interface IMoveType<TBoard> where TBoard : IBoard
    {
        TBoard Apply(TBoard board, Move move);
    }
}
