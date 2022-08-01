using GameLogic.Entities;

namespace GameLogic.Engine
{
    // The interface represents the logic of chess game. 
    public interface IGameLogic<TGameState>
    {
        // Checks if the mate occurs at the current game state.
        bool IsMate(TGameState gameState);

        // Checks if the check occurs at the current game state.
        bool IsCheck(TGameState gameState);

        // Applies the move and returns a new instance of a class implementing the IStandardGameLogic interface, 
        // if the move is valid. Otherwise, returns null.
        TGameState? MakeMove(TGameState gameState, Move move);

        // Finds all valid moves. 
        IEnumerable<Move> FindAllValidMoves(TGameState gameState);
    }
}
