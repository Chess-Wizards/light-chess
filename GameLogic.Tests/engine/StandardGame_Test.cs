using System;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardGame_Test
    {
        [Test]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        public void GameNotationNotChanged(string gameFENNotation)
        {
            // Serialize.
            var gameState = new StandardGame().DeserializeFromFEN(gameFENNotation);
            // Deserialize.
            var gameStateFENNotationOutput = new StandardGame().SerializeToFEN(gameState);

            Assert.AreEqual(gameFENNotation, gameStateFENNotationOutput);
        }
    }
}
