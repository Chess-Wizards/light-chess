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
                var upShift = new Cell(0, 1);
                var rightShift = new Cell(1, 0);
                var downShift = new Cell(0, -1);
                var leftShift = new Cell(-1, 0);

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
        public int NumberShifts { get; } = Int32.MaxValue;

        public EnemyPieceTolerance EnemyPieceTolerance { get; } = EnemyPieceTolerance.MayContain;
    }
}
