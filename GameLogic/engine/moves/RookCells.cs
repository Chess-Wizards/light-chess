using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class RookCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                // up and down over y-axis or files
                // right and left - x-axis or ranks
                var upShift = new Cell(X.Zero, Y.Unit);
                var rightShift = new Cell(X.Unit, Y.Zero);
                var downShift = new Cell(X.Zero, -Y.Unit);
                var leftShift = new Cell(-X.Unit, Y.Zero);

                var shifts = new List<Cell>()
                    {
                        upShift,
                        rightShift,
                        downShift,
                        leftShift
                    };

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shifts},
                    {Color.Black, shifts}
                };
            }
        }
        public int ShiftsNumber { get; } = int.MaxValue;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
