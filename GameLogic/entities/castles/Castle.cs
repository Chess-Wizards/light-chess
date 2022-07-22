namespace GameLogic.Entities.Castles
{
    // Describes a castle.
    //
    // Each castle can be uniquely identified by a pair of color and castle type.
    public struct Castle
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
