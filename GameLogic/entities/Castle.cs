using System;

namespace GameLogic
{
    // The structure contains possible castles. Each castle can
    // be uniquely identified by pair of color and castle type.
    public struct Castle
    {
        public readonly Color Color { get; }
        public readonly CastleType Type { get; }

        public Castle(Color color,
                      CastleType type)
        {
            Color = color;
            Type = type;
        }
    }
}
