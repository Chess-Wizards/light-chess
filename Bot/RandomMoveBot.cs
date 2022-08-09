using GameLogic.Engine;
using GameLogic.Entities;
using GameLogic.Entities.States;

namespace Bot
{
    // Suggests a random move.
    public class RandomMoveBot : IBot
    {
        // Suggest a move. If move is null, then there are no valid moves.
        public Move? SuggestMove(IStandardGameState gameState)
        {
            // A list containing valid moves.
            var moves = new StandardGame().FindAllValidMoves(gameState).ToList(); // why is gameState passing not in the StandardGame ctor?

            if (moves.Any())
            {
                var randomIndex = new Random().Next(0, moves.Count);
                return moves[randomIndex];
            }

            return null;
        }
    }
}
