using GameLogic.Entities;
using GameLogic.Entities.States;

namespace GameLogic.Engine
{
    // The interface represents the logic of chess game. 
    public interface IGameLogic
    {
        // Checks if the mate occurs at the current game state.
        bool IsMate(IStandardGameState gameState);

        // Checks if the check occurs at the current game state.
        bool IsCheck(IStandardGameState gameState);

        // Applies the move and returns a new instance of a class implementing the IStandardGameLogic interface, 
        // if the move is valid. Otherwise, returns null.
        IStandardGameState? MakeMove(IStandardGameState gameState, Move move);

        // Find all valid moves. 
        IEnumerable<Move> FindAllValidMoves(IStandardGameState gameState);
    }
}
