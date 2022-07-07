using System;

namespace GameLogic
{
    // The structure contains possible pieces. Each piece can
    // be uniquely identified by pair of color and piece type.
    public struct Piece
    {
        public readonly Color Color { get; }
        public readonly PieceType Type { get; }


        public Piece(Color color,
                     PieceType type)
        {
            Color = color;
            Type = type;
        }
    }
}
