namespace GameLogic.Entities.Pieces
{
    public class KingShiftConstants : IPieceShiftConstants
    {
        public IEnumerable<Cell> Shifts
        {
            get
            {
                var xs = new[] { -1, 0, 1 };
                var ys = new[] { -1, 0, 1 };
                return
                    (from x in xs
                     from y in ys
                     select new { x, y })
                    .Where((tuple) => !(tuple.x == 0 && tuple.y == 0)) // Exclude tuple corresponding to |cell|.
                    .Select((tuple) => new Cell(tuple.x, tuple.y));
            }
        }
        public bool IsOneShift { get; } = true;
    }
}
