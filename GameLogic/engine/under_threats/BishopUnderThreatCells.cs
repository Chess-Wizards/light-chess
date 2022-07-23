using GameLogic.Entities;

namespace GameLogic.Engine.UnderThreats
{
    public class BishopUnderThreatCells : IPieceUnderThreatCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                // up and down over cells in file.
                // right and left over cells in rank.
                var upRightShift = new Cell(1, 1);
                var downRightShift = new Cell(1, -1);
                var downLeftShift = new Cell(-1, -1);
                var upLeftShift = new Cell(-1, 1);

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
        public bool IsOneShift { get; } = false;
    }
}
