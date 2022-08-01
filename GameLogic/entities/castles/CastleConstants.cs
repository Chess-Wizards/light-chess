namespace GameLogic.Entities.Castles
{
    static public class CastleConstants
    {
        public static readonly Dictionary<Castle, ICastleTypeConstants> mappingCastleToConstant = new()
        {
            {new Castle(Color.White, CastleType.King), new KingCastleConstants(Color.White)},
            {new Castle(Color.White, CastleType.Queen), new QueenCastleConstants(Color.White)},
            {new Castle(Color.Black, CastleType.King), new KingCastleConstants(Color.Black)},
            {new Castle(Color.Black, CastleType.Queen), new QueenCastleConstants(Color.Black)}
        };


    }
}
