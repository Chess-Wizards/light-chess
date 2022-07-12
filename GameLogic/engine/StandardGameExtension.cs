
using System;

namespace GameLogic
{
    public static class StandardGameExtension
    {
        public static string GetFENNotation(this StandardGame game)
        {
            return StandardFENSerializer.SerializeToFEN(game.gameState);
        }
    }
}
