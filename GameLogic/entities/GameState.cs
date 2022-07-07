using System;
using System.Collections.Generic;

namespace GameLogic
{
    // The class represents the game state which has all the required information to continue
    // playing game. 
    //        
    // The game state contains fields/properties inherited from
    // FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation). 
    public class GameState
    {
        // The board contains the piece locations/cells.
        public StandardBoard Board;

        // The color defines the current turn.
        public Color ActiveColor;

        // The list contains available castles.
        public List<Castle> AvaialbleCastleMoves;

        // The en passsant cell.
        public Cell? EnPassantCell;

        // The number of half moves since the last capture or pawn advance.
        public int HalfmoveNumber;

        // The number of the full move. It starts at 1 and is incremented after Black's move.
        public int FullmoveNumber;
    }
}
