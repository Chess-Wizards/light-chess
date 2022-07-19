namespace GameLogic
{
    // Describes a castle.
    //
    // Each castle can be uniquely identified by a pair of color and castle type.
    public struct Castle
    {
        public readonly Color Color { get; }
        public readonly CastleType Type { get; }

        public Castle(Color color, CastleType type)
        {
            Color = color;
            Type = type;
        }
    }
}
