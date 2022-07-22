namespace GameLogic.Entities.Pieces
{
    public class RookShiftConstants : IPieceShiftConstants
    {
        public IEnumerable<Cell> Shifts
        {
            get
            {
                // up and down over y-axis or files
                // right and left - x-axis or ranks
                var upShift = new Cell(0, 1);
                var rightShift = new Cell(1, 0);
                var downShift = new Cell(0, -1);
                var leftShift = new Cell(-1, 0);

                return new List<Cell>()
                    {
                        upShift,
                        rightShift,
                        downShift,
                        leftShift
                    };
            }
        }
        public bool IsOneShift { get; } = false;
    }
}
