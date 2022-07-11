using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardGame_Test
    {
        [Test]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 w KkQq - 2 20",
                  "a1a2 a1a3 a1a4 a1a5 a1a6 a1a7 a1a8 a1b1 c1c2 c1c3 c1c4 c1c5 c1c6 "
                  + "c1c7 c1c8 c1b1 c1d1 c1e1 c1f1 c1g1 c1h1 b2b1 b2b3")]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 b KkQq - 2 20",
                  "a8a1 a8a2 a8a3 a8a4 a8a5 a8a6 a8a7 a8b8 c8c1 c8c2 c8c3 c8c4 c8c5 "
                  + "c8c6 c8c7 c8b8 c8d8 c8e8 c8f8 c8g8 c8h8 b7b6 b7b8")]
        public void FindAllValidMovesCorrect(string board,
                                             string cellsUnderThreatNotParsed)
        {
            var expectedCellsToMove = cellsUnderThreatNotParsed.Split(' ')
                                                               .OrderBy(notation => notation)
                                                               .ToList();

            var cellsToMove = new StandardGame(board).FindAllValidMoves()
                                                     .Select(move => $"{StandardFENSerializer.CellToNotation(move.StartCell)}"
                                                                     + $"{StandardFENSerializer.CellToNotation(move.EndCell)}")
                                                     .OrderBy(notation => notation)
                                                     .ToList();

            Assert.That(cellsToMove, Is.EqualTo(expectedCellsToMove));
        }

        [Test]
        // Checks
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 b KkQq - 2 20", true)]
        [TestCase("rkr5/8/8/8/8/8/1K6/R1R5 b KkQq - 2 20", true)]
        [TestCase("r1r5/1k6/8/8/8/8/2K5/R1R5 b KkQq - 2 20", false)]
        [TestCase("r1r5/1k6/8/8/8/8/K7/R1R5 b KkQq - 2 20", false)]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 w KkQq - 2 20", true)]
        [TestCase("r1r5/1k6/8/8/8/8/8/RKR5 w KkQq - 2 20", true)]
        [TestCase("r1r5/2k5/8/8/8/8/1K6/R1R5 w KkQq - 2 20", false)]
        [TestCase("r1r5/k7/8/8/8/8/1K6/R1R5 w KkQq - 2 20", false)]
        // Pawn locations
        [TestCase("r1r5/1k6/8/pPpPpPpP/8/8/1K6/R1R5 w KkQq - 2 20", true)]
        [TestCase("r1r4p/k7/8/8/8/8/1K6/R1R5 w KkQq - 2 20", false)]
        [TestCase("r1r5/k7/8/8/8/8/1K6/R1R4P w KkQq - 2 20", false)]
        // Number of kings
        [TestCase("r1r5/1k6/8/8/8/8/1K5K/R1R5 w KkQq - 2 20", false)]
        // Number of enemy kings
        [TestCase("r1r5/1k5k/8/8/8/8/1K6/R1R5 w KkQq - 2 20", false)]
        public void IsValidCorrect(string board,
                                   bool isValid)
        {
            Assert.That(new StandardGame(board).IsValid(), Is.EqualTo(isValid));
        }

        [Test]
        [TestCase("8/8/8/8/8/5k2/6q1/6K1 w - - 2 20", true)]
        [TestCase("8/8/8/8/8/7q/8/5k1K w - - 2 20", true)]
        [TestCase("8/8/8/8/8/7r/8/5k1K w - - 2 20", true)]
        [TestCase("8/8/8/8/8/4k3/6q1/6K1 w - - 2 20", false)]
        [TestCase("8/8/8/8/8/5K2/6Q1/6k1 b - - 2 20", true)]
        [TestCase("8/8/8/8/8/7Q/8/5K1k b - - 2 20", true)]
        [TestCase("8/8/8/8/8/7R/8/5K1k b - - 2 20", true)]
        [TestCase("8/8/8/8/8/4K3/6Q1/6k1 b - - 2 20", false)]
        public void IsMateCorrect(string board,
                                  bool isMate)
        {
            Assert.That(new StandardGame(board).IsMate(), Is.EqualTo(isMate));
        }
    }
}
