using System;
using System.Collections.Generic;

namespace GameLogic
{
    // The interface represents the logic of chess game. 
    public interface IStandardGameLogic
    {
        // Checks if the mate occurs at current game state.
        bool IsMate(StandardGameState gameState);

        // Checks if the check occurs at current game state.
        bool IsCheck(StandardGameState gameState);

        // Applies the move and returns a new instance of StandardGameState, if the move is valid. Otherwise, returns null.
        StandardGameState MakeMove(StandardGameState? gameState,
                                   Move move);

        // Checks if the game state is possible.
        bool IsGameStateValid(StandardGameState gameState);

        // Find all moves. Moves can be invalid.
        List<Move> FindAllMoves(StandardGameState gameState);

        // Find all cells under threat produced by enemy
        List<Cell> FindAllCellsUnderThreat(StandardGameState gameState);
    }
}
