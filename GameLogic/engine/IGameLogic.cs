using System;
using System.Collections.Generic;

namespace GameLogic
{
    // The interface represents the logic of chess game. 
    public interface IGameLogic
    {
        // Checks if the mate occurs at the current game state.
        bool IsMate(StandardGameState gameState);

        // Checks if the check occurs at the current game state.
        bool IsCheck(StandardGameState gameState);

        // Applies the move and returns a new instance of a class implementing the IStandardGameLogic interface, 
        // if the move is valid. Otherwise, returns null.
        StandardGameState? MakeMove(StandardGameState gameState, Move move);

        // Find all valid moves. 
        List<Move> FindAllValidMoves(StandardGameState gameState);
    }
}
