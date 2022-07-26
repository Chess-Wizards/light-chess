using GameLogic.Entities;

namespace GameLogic.Engine.Moves
{
    public interface IPieceCells
    {
        IDictionary<Color, IEnumerable<Cell>> Shifts { get; }

        int NumberShifts { get; }

        // Defines if we may/must/must not capture enemy pieces.
        EnemyPieceTolerance EnemyPieceTolerance { get; }

        IEnumerable<Cell> GetCells(Cell cell,
                                   Color activeColor,
                                   IEnumerable<Cell> pieceCells,
                                   IEnumerable<Cell> enemyPieceCells,
                                   Func<Cell, bool> IsOnBoard)
        {
            // List to save cells 'under threat'.
            var cells = new List<Cell>();

            int currentNumberShifts = 1;
            // Iterate over shifts.

            foreach (var shift in Shifts[activeColor])
            {
                var currentCell = cell;
                // Iterate once if |oneShift| is set to true, otherwise iterate until obstacles are found.
                while (true)
                {
                    currentCell = currentCell + shift;
                    if (!IsOnBoard(currentCell) || pieceCells.Contains(currentCell))
                    {
                        break;
                    }

                    cells.Add(currentCell);
                    currentNumberShifts += 1;
                    if (currentNumberShifts > NumberShifts || enemyPieceCells.Contains(currentCell))
                    {
                        break;
                    }
                }
            }

            if (EnemyPieceTolerance == EnemyPieceTolerance.MustContain)
            {
                return cells.Where(cell => enemyPieceCells.Contains(cell));
            }
            else if (EnemyPieceTolerance == EnemyPieceTolerance.MustNotContain)
            {
                return cells.Where(cell => !enemyPieceCells.Contains(cell));
            }

            return cells;
        }
    }
}
