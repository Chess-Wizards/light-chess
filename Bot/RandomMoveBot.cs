using GameLogic.Engine;
using GameLogic.Entities;
using GameLogic.Entities.States;

// TODO: think about passing IStandardGame to Bot ctor

namespace Bot
{
    // Suggests a random move.
    public class RandomMoveBot : IBot
    {
        public Move? SuggestMove(IStandardGameState gameState)
        {
            // A list containing valid moves.
            var moves = new StandardGame().FindAllValidMoves(gameState).ToList();
            if (moves.Any())
            {
                var randomIndex = new Random().Next(0, moves.Count);
                return moves[randomIndex];
            }

            return null;
        }
    }
}
