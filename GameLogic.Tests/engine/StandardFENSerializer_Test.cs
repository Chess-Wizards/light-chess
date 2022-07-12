using System;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardFENSerializer_Test
    {
        [Test]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        public void GameNotationNotChanged(string gameFENNotation)
        {
            // Serialize.
            var gameState = StandardFENSerializer.DeserializeFromFEN(gameFENNotation);
            // Deserialize.
            var gameStateFENNotationOutput = StandardFENSerializer.SerializeToFEN(gameState);
        }

        [Test]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")]
        public void BoardNotationNotChanged(string boardFENNotation)
        {
            // Serialize.
            var board = StandardFENSerializer.NotationToBoard(boardFENNotation);
            // Deserialize.
            var boardFENNotationOutput = StandardFENSerializer.BoardToNotation(board);

            Assert.That(boardFENNotationOutput, Is.EqualTo(boardFENNotation));
        }

        [Test]
        [TestCase("w")]
        [TestCase("b")]
        public void ColorNotationNotChanged(string colorFENNotation)
        {
            // Serialize.
            var color = StandardFENSerializer.NotationToColor(colorFENNotation);
            // Deserialize.
            var colorFENNotationOutput = StandardFENSerializer.ColorToNotation(color);

            Assert.That(colorFENNotationOutput, Is.EqualTo(colorFENNotation));
        }

        [Test]
        [TestCase("-")]
        [TestCase("QqKk")]
        [TestCase("KQ")]
        [TestCase("Qk")]
        [TestCase("Kq")]
        public void CastleNotationNotChanged(string castleFENNotation)
        {
            // Serialize.
            var castle = StandardFENSerializer.NotationToCastle(castleFENNotation);
            // Deserialize.
            var castleFENNotationOutput = StandardFENSerializer.CastleToNotation(castle);

            Assert.That(castleFENNotationOutput, Is.EqualTo(castleFENNotation));
        }

        [Test]
        [TestCase("a1")]
        [TestCase("a8")]
        [TestCase("h1")]
        [TestCase("h5")]
        [TestCase("d4")]
        [TestCase("d5")]
        [TestCase("e4")]
        [TestCase("e5")]
        public void CellNotationNotChanged(string cellFENNotation)
        {
            // Serialize.
            var cell = StandardFENSerializer.NotationToCell(cellFENNotation);
            // Deserialize.
            var cellFENNotationOutput = StandardFENSerializer.CellToNotation(cell);

            Assert.That(cellFENNotationOutput, Is.EqualTo(cellFENNotation));
        }

        [Test]
        [TestCase("a1-a8")]
        [TestCase("h1-h8")]
        [TestCase("b1-b4")]
        [TestCase("e5-f7")]
        [TestCase("h1-g1")]
        [TestCase("e6-e8")]
        [TestCase("e7-e8R")]
        [TestCase("e7-e8N")]
        [TestCase("e7-e8Q")]
        [TestCase("e7-e8B")]
        public void MoveNotationNotChanged(string moveFENNotation)
        {
            // Serialize.
            var move = StandardFENSerializer.NotationToMove(moveFENNotation);
            // Deserialize.
            var moveFENNotationOutput = StandardFENSerializer.MoveToNotation(move);

            Assert.That(moveFENNotationOutput, Is.EqualTo(moveFENNotation));
        }
    }
}
