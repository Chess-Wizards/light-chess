namespace GameLogic.Entities.Pieces
{
    // Each piece can be uniquely identified by the pair: {color, piece type}.
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
