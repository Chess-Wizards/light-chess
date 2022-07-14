using GameLogic;

namespace Bot
{
    // This class inputs a FEN notation and suggest a random move from a list of valid moves.
    public class RandomMoveBot: IBot
    {
        public string gameStateNotation;

        public RandomMoveBot(string gameStateNotation)
        {
            this.gameStateNotation = gameStateNotation;
        }

        // Suggest a move.
        //
        // Returns
        // -------
        // A random move or null.
        public Move? SuggestMove()
        {
            // A list containing valid moves.
            var moves = new StandardGame(gameStateNotation).FindAllValidMoves();

            if (moves.Count == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(0, moves.Count);
            return moves[randomIndex];
        }
    }
}