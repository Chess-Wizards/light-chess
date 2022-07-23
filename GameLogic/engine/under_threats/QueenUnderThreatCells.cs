using GameLogic.Entities;

namespace GameLogic.Engine.UnderThreats
{
    public class QueenUnderThreatCells : IPieceUnderThreatCells
    {
        public IDictionary<Color, IEnumerable<Cell>> Shifts
        {
            get
            {
                var bishopShifts = new BishopUnderThreatCells().Shifts[Color.White].ToList();
                var rookShifts = new RookUnderThreatCells().Shifts[Color.White].ToList();
                var shifts = bishopShifts.Concat(rookShifts).ToList();
                
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
