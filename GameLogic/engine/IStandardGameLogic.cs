using System;
using System.Collections.Generic;

namespace GameLogic
{
    // The interface represents the logic of chess game. 
    public interface IStandardGameLogic
    {
        // Checks if the mate occurs at current game state.
        bool IsMate();

        // Checks if the check occurs at current game state.
        bool IsCheck();

        // Applies the move and returns a new instance of StandardGameState, if the move is valid. Otherwise, returns null.
        StandardGameState MakeMove(Move move);

        // Checks if the current game state is valid.
        bool IsValid();

        // Find all valid moves. Moves can be invalid.
        List<Move> FindAllValidMoves(StandardGameState gameState);
    }
}
