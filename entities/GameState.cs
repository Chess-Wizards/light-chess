using System;
using System.Collections.Generic;

namespace LightChess
{

    public class GameState
    {
        /*
            The class represents the game state which has all the required information to continue
            playing game. 
            
            P.S. The game state contains fields/properties inherited from
            FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation). 
        */

        // The board contains the piece locations/cells.
        StandardBoard board;
        // The color defines the current turn.
        Color ActiveColor;
        // The list contains available castles.
        List<Castle> AvaialbleCastles;
        // The list contains avaialble enpassant moves.
        List<Move> EnPassantMoves;
        // The number of half moves since the last capture or pawn advance.
        int HalfmoveNumber;
        // The number of the full move. It starts at 1 and is incremented after Black's move.
        int FullmoveNumber;
    }
}