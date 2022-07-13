using GameLogic;

namespace Bot
{
    public interface IBot
    {
        // Suggest a move.
        Move? SuggestMove();
    }
}