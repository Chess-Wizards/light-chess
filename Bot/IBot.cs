using GameLogic.Entities;
using GameLogic.Entities.States;

namespace Bot
{
    public interface IBot
    {
        // Suggests a move. If move is null, then there are no valid moves.
        Move? SuggestMove(IStandardGameState gameState);
    }
}
