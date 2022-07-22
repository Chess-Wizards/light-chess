namespace GameLogic.Entities.Castles
{
    public class CastleConstants
    {
        public readonly Dictionary<Castle, ICastleTypeConstants> mappingCastleToConstant = new()
        {
            {new Castle(Color.White, CastleType.King), new WhiteKingCastleConstants()},
            {new Castle(Color.White, CastleType.Queen), new WhiteQueenCastleConstants()},
            {new Castle(Color.Black, CastleType.King), new BlackKingCastleConstants()},
            {new Castle(Color.Black, CastleType.Queen), new BlackQueenCastleConstants()}
        };


    }
}
