using GameLogic.Entities;
using GameLogic.Entities.States;

namespace Bot
{
    public interface IBot
    {
        // Move Suggestion.
        Move? SuggestMove(IStandardGameState gameState);
    }
}
