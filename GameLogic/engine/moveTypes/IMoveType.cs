using GameLogic.Entities;
using GameLogic.Entities.Boards;

namespace GameLogic.Engine.MoveTypes
{
    public interface IMoveType<TBoard> where TBoard : IBoard // maybe also "out TBoard" ?
    {
        TBoard Apply(TBoard board, Move move);
    }
}
