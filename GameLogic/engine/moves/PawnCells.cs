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

                var shiftsWhite = new List<Cell>()
                    {
                        shiftWhite
                    };

                var shiftsBlack = new List<Cell>()
                    {
                        shiftBlack
                    };

                return new Dictionary<Color, IEnumerable<Cell>>()
                {
                    {Color.White, shiftsWhite},
                    {Color.Black, shiftsBlack}
                };
            }
        }
        public int NumberShifts { get; }

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MustNotContain;

        public PawnCells(int numberShifts)
        {
            NumberShifts = numberShifts;
        }
    }
}
