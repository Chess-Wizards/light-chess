using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class PawnCells : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                var shiftWhite = new Cell(X.Zero, Y.Unit);
                var shiftBlack = new Cell(X.Zero, -Y.Unit);

                var whiteShifts = new List<Cell>()
                    {
                        shiftWhite
                    };

                var blackShifts = new List<Cell>()
                    {
                        shiftBlack
                    };

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, whiteShifts},
                    {Color.Black, blackShifts}
                };
            }
        }
        public int ShiftsNumber { get; }

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MustNotContain;

        public PawnCells(int shiftsNumber)
        {
            ShiftsNumber = shiftsNumber;
        }
    }
}
