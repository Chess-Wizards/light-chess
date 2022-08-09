using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class BishopCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                // up and down over cells in file.
                // right and left over cells in rank.
                var upRightShift = new Cell(X.Unit, Y.Unit);
                var downRightShift = new Cell(X.Unit, -Y.Unit);
                var downLeftShift = new Cell(-X.Unit, -Y.Unit);
                var upLeftShift = new Cell(-X.Unit, Y.Unit);

                var shifts = new List<Cell>()
                    {
                        upRightShift,
                        downRightShift,
                        downLeftShift,
                        upLeftShift
                    };

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shifts},
                    {Color.Black, shifts}
                };
            }
        }
        public int ShiftsNumber { get; } = int.MaxValue;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContainOnce;
    }
}
