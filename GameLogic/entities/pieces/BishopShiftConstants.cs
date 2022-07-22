namespace GameLogic.Entities.Pieces
{
    public class BishopShiftConstants : IPieceShiftConstants
    {
        public IEnumerable<Cell> Shifts
        {
            get
            {
                // up and down over cells in file.
                // right and left over cells in rank.
                var upRightShift = new Cell(1, 1);
                var downRightShift = new Cell(1, -1);
                var downLeftShift = new Cell(-1, -1);
                var upLeftShift = new Cell(-1, 1);

                return new List<Cell>()
                    {
                        upRightShift,
                        downRightShift,
                        downLeftShift,
                        upLeftShift
                    };
            }
        }
        public bool IsOneShift { get; } = false;
    }
}
