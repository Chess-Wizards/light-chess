using System;

namespace GameLogic
{
    // The structure defines move. Each move can
    // be uniquely identified by a triplet of start cell, end cell, and end piece type.
    // For example, the pawn can be promoted to another piece. Therefore, the piece type is required.
    public struct Move
    {
        public readonly Cell StartCell { get; }
        public readonly Cell EndCell { get; }
        public readonly PieceType? EndPieceType { get; }


        public Move(Cell startCell,
                    Cell endCell,
                    PieceType? endPieceType = null)
        {
            StartCell = startCell;
            EndCell = endCell;
            EndPieceType = endPieceType;
        }
    }
}
