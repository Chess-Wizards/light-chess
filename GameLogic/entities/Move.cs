using System;

namespace GameLogic
{
    // The structure defines move. Each move can
    // be uniquely identified by pair of start and end cells.
    public struct Move
    {
        public readonly Cell StartCell { get; }
        public readonly Cell EndCell { get; }

        public Move(Cell startCell,
                    Cell endCell)
        {
            StartCell = startCell;
            EndCell = endCell;
        }
    }
}