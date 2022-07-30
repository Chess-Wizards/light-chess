namespace Communication.Protocols.UCI
{
    public class PositionCommandConstants
    {
        public string StartPositionIndicator { get { return "startpos"; } }
        public int FirstFENNotationIndex { get { return 2; } }
        public int NotationLength { get { return 6; } }

        public int FirstMoveIndexWithStartPositionIndicator { get { return 3; } }

        public int FirstMoveIndexWithoutStartPositionIndicator { get { return 9; } }
    }
}
