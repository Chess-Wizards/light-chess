namespace GameLogic.Entities.Castlings
{
    // Each castling move can be uniquely identified by the pair: {color, side}.
    public struct Castling
    {
        public Color Color { get; }
        public CastlingType Type { get; }

        public Castling(Color color, CastlingType type)
        {
            Color = color;
            Type = type;
        }
    }
}
