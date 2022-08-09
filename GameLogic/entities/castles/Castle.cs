namespace GameLogic.Entities.Castles
{
    // Each castling move can be uniquely identified by pair of color and castle side.
    public struct Castle // maybe rename to "Castling"
    {
        public Color Color { get; }

        public CastleType Type { get; }

        public Castle(Color color, CastleType type)
        {
            Color = color;
            Type = type;
        }
    }
}
