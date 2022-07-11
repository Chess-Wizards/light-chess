using System;
using System.Collections.Generic;

namespace GameLogic
{
    // The interface represents the logic of chess game. 
    public interface IStandardGameLogic<T>
    {
        // Checks if the mate occurs at the current game state.
        bool IsMate();

        // Checks if the check occurs at the current game state.
        bool IsCheck();

        // Applies the move and returns a new instance of a class implementing the IStandardGameLogic interface, 
        // if the move is valid. Otherwise, returns null.
        T? MakeMove(Move move);

        // Checks if the current game state is valid.
        bool IsValid();

        // Find all valid moves. 
        List<Move> FindAllValidMoves();
    }
}
