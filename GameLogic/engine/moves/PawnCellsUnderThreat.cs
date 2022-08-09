using GameLogic.Entities;

// TODO: Dear @SherlockKA, please try to get rid of this class.

namespace GameLogic.Engine.Moves
{
    public class PawnCellsUnderThreat : IPieceCells
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
        public int ShiftsNumber { get; } = 1;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
