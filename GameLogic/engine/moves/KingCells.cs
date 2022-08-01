using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class KingCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                var xs = new[] { -X.Unit, X.Zero, X.Unit };
                var ys = new[] { -Y.Unit, Y.Zero, Y.Unit };
                var shifts =
                    (from x in xs
                     from y in ys
                     select new { x, y })
                    .Where((tuple) => !(tuple.x == X.Zero && tuple.y == Y.Zero)) // Exclude tuple corresponding to |cell|.
                    .Select((tuple) => new Cell(tuple.x, tuple.y));

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shifts},
                    {Color.Black, shifts}
                };
            }
        }
        public int NumberShifts { get; } = 1;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
