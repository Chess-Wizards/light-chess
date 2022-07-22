namespace GameLogic.Entities.Pieces
{
    public class KnightShiftConstants : IPieceShiftConstants
    {
        public IEnumerable<Cell> Shifts
        {
            get
            {
                var xs = new[] { -2, -1, 1, 2 };
                var ys = new[] { -2, -1, 1, 2 };
                return
                    (from x in xs
                     from y in ys
                     select new { x, y }) // Get all combinations.
                    .Where((tuple) => Math.Abs(tuple.x) != Math.Abs(tuple.y))
                    .Select((tuple) => new Cell(tuple.x, tuple.y));
            }
        }
        public bool IsOneShift { get; } = true;
    }
}
