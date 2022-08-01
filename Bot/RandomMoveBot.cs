using GameLogic.Entities;
using GameLogic.Entities.States;
using GameLogic.Engine;

namespace Bot
{
    // Suggests a random move.
    public class RandomMoveBot : IBot
    {

        // Suggest a move. If move is null, then there are no valid moves.
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
