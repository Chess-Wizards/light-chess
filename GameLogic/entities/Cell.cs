namespace GameLogic.Entities
{
    // Contains possible cell coordinates. 
    // Each cell can be uniquely identified by pair of coordinates X and Y.
    public struct Cell
    {
        public int X { get; }
        public int Y { get; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Cell operator +(Cell cell1, Cell cell2)
        {
            return new Cell(cell1.X + cell2.X, cell1.Y + cell2.Y);
        }

        public static bool operator ==(Cell cell1, Cell cell2)
        {
            return cell1.Equals(cell2);
        }

        public static bool operator !=(Cell cell1, Cell cell2)
        {
            return !cell1.Equals(cell2);
        }

        public override bool Equals(object obj)
        {
            var cell = (Cell)obj;
            if (cell == null)
            {
                return false;
            }

            return X == cell.X && Y == cell.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
