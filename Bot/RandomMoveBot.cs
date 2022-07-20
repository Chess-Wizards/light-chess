using GameLogic.Entities;
using GameLogic.Entities.States;
using GameLogic.Engine;

namespace Bot
{
    // This class inputs a FEN notation and suggest a random move from a list of valid moves.
    public class RandomMoveBot : IBot
    {

        // Suggest a move.
        //
        // Parameters
        // ----------
        // gameState: A standard game state.
        //
        // Returns
        // -------
        // A random move or null (There are no valid moves).
        public Move? SuggestMove(StandardGameState gameState)
        {
            // A list containing valid moves.
            var moves = new StandardGame().FindAllValidMoves(gameState);

            if (moves.Count == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(0, moves.Count);
            return moves[randomIndex];
        }
    }
}
