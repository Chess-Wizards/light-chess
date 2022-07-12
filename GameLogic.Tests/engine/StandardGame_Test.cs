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
                  "a1-a2 a1-a3 a1-a4 a1-a5 a1-a6 a1-a7 a1-a8 a1-b1 c1-c2 c1-c3 c1-c4 c1-c5 c1-c6"
                  + " c1-c7 c1-c8 c1-b1 c1-d1 c1-e1 c1-f1 c1-g1 c1-h1 b2-b1 b2-b3")]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 b KkQq - 2 20",
                  "a8-a1 a8-a2 a8-a3 a8-a4 a8-a5 a8-a6 a8-a7 a8-b8 c8-c1 c8-c2 c8-c3 c8-c4 c8-c5"
                  + " c8-c6 c8-c7 c8-b8 c8-d8 c8-e8 c8-f8 c8-g8 c8-h8 b7-b6 b7-b8")]
        public void FindAllValidMovesCorrect(string board,
                                             string cellsUnderThreatNotParsed)
        {
            var expectedCellsToMove = cellsUnderThreatNotParsed.Split(' ')
                                                               .OrderBy(notation => notation)
                                                               .ToList();

            var cellsToMove = new StandardGame(board).FindAllValidMoves()
                                                     .Select(move => StandardFENSerializer.MoveToNotation(move))
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
