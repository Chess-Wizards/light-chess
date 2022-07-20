using GameLogic.Entities;
using GameLogic.Entities.States;

namespace Bot
{
    public interface IBot
    {
        // Suggest a move.
        Move? SuggestMove(StandardGameState gameState);
    }
}
