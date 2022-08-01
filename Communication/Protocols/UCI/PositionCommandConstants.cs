namespace Communication.Protocols.UCI
{
    public class PositionCommandConstants
    {
        public const string StartPositionIndicator = "startpos";
        public const int FirstFENNotationIndex = 2;
        public const int NotationLength = 6;
        public const int FirstMoveIndexWithStartPositionIndicator = 3;
        public const int FirstMoveIndexWithoutStartPositionIndicator = 9;
    }
}
