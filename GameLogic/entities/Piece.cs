namespace GameLogic.Entities
{
    // Possible pieces. Each piece can
    // be uniquely identified by pair of color and piece type.
    public struct Piece
    {
        public Color Color { get; }
        public PieceType Type { get; }

        public Piece(Color color, PieceType type)
        {
            Color = color;
            Type = type;
        }
    }
}
