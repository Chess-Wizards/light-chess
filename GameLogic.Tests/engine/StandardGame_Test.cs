using NUnit.Framework;
using GameLogic.Engine;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardGame_Test
    {
        [Test]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 w - - 2 20",
                  "a1a2 a1a3 a1a4 a1a5 a1a6 a1a7 a1a8 a1b1 c1c2 c1c3 c1c4 c1c5 c1c6"
                  + " c1c7 c1c8 c1b1 c1d1 c1e1 c1f1 c1g1 c1h1 b2b1 b2b3")]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 b - - 2 20",
                  "a8a1 a8a2 a8a3 a8a4 a8a5 a8a6 a8a7 a8b8 c8c1 c8c2 c8c3 c8c4 c8c5"
                  + " c8c6 c8c7 c8b8 c8d8 c8e8 c8f8 c8g8 c8h8 b7b6 b7b8")]
        // Checks white castle moves.
        [TestCase("r3k2r/p6p/8/8/8/8/P6P/R3K2R w KkQq - 2 20",
                  "a1b1 a1c1 a1d1 h1g1 h1f1 e1c1 e1d1 e1d2 e1e2 e1f2 e1f1 e1g1 a2a3 a2a4 h2h3 h2h4")]
        // Checks black castle moves.
        [TestCase("r3k2r/p6p/8/8/8/8/P6P/R3K2R b KkQq - 2 20",
                  "a8b8 a8c8 a8d8 h8g8 h8f8 e8c8 e8d8 e8d7 e8e7 e8f7 e8f8 e8g8 a7a6 a7a5 h7h6 h7h5")]
        // Checks white en passant moves.
        [TestCase("4k3/8/8/5PpP/8/8/8/4K3 w - g6 2 20",
                  "e1d1 e1d2 e1e2 e1f2 e1f1 f5f6 f5g6 h5h6 h5g6")]
        [TestCase("4k3/8/8/5PpP/8/8/8/4K3 w - - 2 20",
                  "e1d1 e1d2 e1e2 e1f2 e1f1 f5f6 h5h6")]
        // Checks black en passant moves.
        [TestCase("4k3/8/8/8/pPp/8/8/4K3 b - b3 2 20",
                  "e8d8 e8d7 e8e7 e8f7 e8f8 a4a3 a4b3 c4c3 c4b3")]
        [TestCase("4k3/8/8/8/pPp/8/8/4K3 b - - 2 20",
                  "e8d8 e8d7 e8e7 e8f7 e8f8 a4a3 c4c3")]
        public void FindAllValidMovesCorrect(string gameStateNotation, string cellsUnderThreatNotParsed)
        {
            var expectedCellsToMove = cellsUnderThreatNotParsed.Split(' ')
                                                               .OrderBy(notation => notation)
                                                               .ToList();
            var gameState = StandardFENSerializer.DeserializeFromFEN(gameStateNotation);

            var cellsToMove = new StandardGame().FindAllValidMoves(gameState)
                                                .Select(move => StandardFENSerializer.MoveToNotation(move))
                                                .OrderBy(notation => notation)
                                                .ToList();

            CollectionAssert.AreEqual(cellsToMove, expectedCellsToMove);
        }

        [Test]
        // Checks
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 b - - 2 20", true)]
        [TestCase("rkr5/8/8/8/8/8/1K6/R1R5 b - - 2 20", true)]
        [TestCase("r1r5/1k6/8/8/8/8/2K5/R1R5 b - - 2 20", false)]
        [TestCase("r1r5/1k6/8/8/8/8/K7/R1R5 b - - 2 20", false)]
        [TestCase("r1r5/1k6/8/8/8/8/1K6/R1R5 w - - 2 20", true)]
        [TestCase("r1r5/1k6/8/8/8/8/8/RKR5 w - - 2 20", true)]
        [TestCase("r1r5/2k5/8/8/8/8/1K6/R1R5 w - - 2 20", false)]
        [TestCase("r1r5/k7/8/8/8/8/1K6/R1R5 w - - 2 20", false)]
        // Pawn locations
        [TestCase("r1r5/1k6/8/pPpPpPpP/8/8/1K6/R1R5 w - - 2 20", true)]
        [TestCase("r1r4p/k7/8/8/8/8/1K6/R1R5 w - - 2 20", false)]
        [TestCase("r1r5/k7/8/8/8/8/1K6/R1R4P w - - 2 20", false)]
        // Number of kings
        [TestCase("r1r5/1k6/8/8/8/8/1K5K/R1R5 w - - 2 20", false)]
        // Number of enemy kings
        [TestCase("r1r5/1k5k/8/8/8/8/1K6/R1R5 w - - 2 20", false)]
        public void IsValidCorrect(string gameStateNotation, bool expectedIsValid)
        {
            var gameState = StandardFENSerializer.DeserializeFromFEN(gameStateNotation);

            try
            {
                new StandardGame().FindAllValidMoves(gameState);
            }
            catch (ArgumentException)
            {
                Assert.False(expectedIsValid);
            }
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
        public void IsMateCorrect(string gameStateNotation, bool expectedIsMate)
        {
            var gameState = StandardFENSerializer.DeserializeFromFEN(gameStateNotation);
            Assert.That(expectedIsMate, Is.EqualTo(new StandardGame().IsMate(gameState)));
        }
    }
}
