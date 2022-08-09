namespace GameLogic.Entities.Castlings
{
    public static class CastlingConstants
    {
        public static readonly Dictionary<Castling, ICastlingTypeConstants> castlingToConstantsMap = new()
        {
            {new Castling(Color.White, CastlingType.KingSide), new KingSideCastlingConstants(Color.White)},
            {new Castling(Color.White, CastlingType.QueenSide), new QueenSideCastlingConstants(Color.White)},
            {new Castling(Color.Black, CastlingType.KingSide), new KingSideCastlingConstants(Color.Black)},
            {new Castling(Color.Black, CastlingType.QueenSide), new QueenSideCastlingConstants(Color.Black)}
        };
    }
}
