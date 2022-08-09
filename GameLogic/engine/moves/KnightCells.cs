using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class KnightCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                var xs = new[] { -2 * X.Unit, -X.Unit, X.Unit, 2 * X.Unit };
                var ys = new[] { -2 * Y.Unit, -Y.Unit, Y.Unit, 2 * Y.Unit };
                var shifts =
                    (from x in xs
                     from y in ys
                     select new { x, y }) // Get all combinations.
                    .Where((tuple) => Math.Abs(tuple.x) != Math.Abs(tuple.y))
                    .Select((tuple) => new Cell(tuple.x, tuple.y));

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shifts},
                    {Color.Black, shifts}
                };
            }
        }
        public int ShiftsNumber { get; } = 1;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
