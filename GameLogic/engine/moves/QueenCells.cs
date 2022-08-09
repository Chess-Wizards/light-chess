using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class QueenCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                // No matter what color to use.
                var bishopShifts = new BishopCells().Shifts[Color.White];
                var rookShifts = new RookCells().Shifts[Color.White];
                var shifts = bishopShifts.Concat(rookShifts);

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
