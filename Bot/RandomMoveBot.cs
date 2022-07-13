using GameLogic;

namespace Bot
{
    public class RandomMoveBot: IBot
    {
        public StandardGameState gameState;

        public RandomMoveBot(string fenNotation)
        {
            this.gameState = StandardFENSerializer.DeserializeFromFEN(fenNotation);
        }

        public RandomMoveBot(StandardGameState gameState)
        {
            this.gameState = gameState;
        }

        // Suggest a move.
        //
        // Returns
        // -------
        // A random move or null.
        public Move? SuggestMove()
        {
            var moves = new StandardGame(gameState).FindAllValidMoves();

            if (moves.Count == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(0, moves.Count);
            return moves[randomIndex];
        }
    }
}