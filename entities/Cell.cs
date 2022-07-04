using System;

namespace LightChess
{
    public struct Cell
    {
        /*
            The structure contains possible cell coordinates. 
            Each cell can be uniquely identified by pair of 
            coordinates X and Y.
        */
        
        public readonly int X {get; }
        public readonly int Y {get; }

        public Cell(int x, 
                    int y)
        {
            X = x;
            Y = y;
        }

        public static Cell operator+(Cell cell1, 
                                     Cell cell2)
        {
            return new Cell(cell1.X+cell2.X, cell1.Y+cell2.Y);
        }

        public static bool operator ==(Cell cell1, 
                                       Cell cell2) 
        {
            return cell1.Equals(cell2);
        }

        public static bool operator !=(Cell cell1, 
                                       Cell cell2) 
        {
        return !cell1.Equals(cell2);
        }
    }
}
