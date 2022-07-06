using System;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class SerializeHelper_Test
    {
        [Test]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")]
        public void BoardNotationNotChanged(string boardFENNotation)
        {
            // Serialize.
            var board = SerializeHelper.NotationToBoard(boardFENNotation);
            // Deserialize.
            var boardFENNotationOutput = SerializeHelper.BoardToNotation(board);

            Assert.AreEqual(boardFENNotation, boardFENNotationOutput);
        }

        [Test]
        [TestCase("w")]
        [TestCase("b")]
        public void ColorNotationNotChanged(string colorFENNotation)
        {
            // Serialize.
            var color = SerializeHelper.NotationToColor(colorFENNotation);
            // Deserialize.
            var colorFENNotationOutput = SerializeHelper.ColorToNotation(color);

            Assert.AreEqual(colorFENNotation, colorFENNotationOutput);
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
            var castle = SerializeHelper.NotationToCastle(castleFENNotation);
            // Deserialize.
            var castleFENNotationOutput = SerializeHelper.CastleToNotation(castle);

            Assert.AreEqual(castleFENNotation, castleFENNotationOutput);
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
            var cell = SerializeHelper.NotationToCell(cellFENNotation);
            // Deserialize.
            var cellFENNotationOutput = SerializeHelper.CellToNotation(cell);

            Assert.AreEqual(cellFENNotation, cellFENNotationOutput);
        }
    }
}
