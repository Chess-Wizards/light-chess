using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public class PawnCellsUnderThreat : IPieceCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {

                var leftShiftWhite = new Cell(-1, 1);
                var rightShiftWhite = new Cell(1, 1);
                var leftShiftBlack = new Cell(-1, -1);
                var rightShiftBlack = new Cell(1, -1);

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

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
