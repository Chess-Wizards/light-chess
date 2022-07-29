using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class PawnCellsCapture : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                var leftShiftWhite = new Cell(-X.Unit, Y.Unit);
                var rightShiftWhite = new Cell(X.Unit, Y.Unit);
                var leftShiftBlack = new Cell(-X.Unit, -Y.Unit);
                var rightShiftBlack = new Cell(X.Unit, -Y.Unit);

                var shiftsWhite = new List<Cell>()
                    {
                        leftShiftWhite,
                        rightShiftWhite
                    };

                var shiftsBlack = new List<Cell>()
                    {
                        leftShiftBlack,
                        rightShiftBlack
                    };

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shiftsWhite},
                    {Color.Black, shiftsBlack}
                };
            }
        }
        public int NumberShifts { get; } = 1;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MustContain;
    }
}
