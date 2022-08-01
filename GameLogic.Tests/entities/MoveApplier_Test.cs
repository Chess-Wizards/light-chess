using GameLogic.Engine;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class MoveApplier_Test
    {
        [Test]
        // 1. White moves
        // King castle.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R4RK1 b kq - 3 21",
                  "e1g1")]
        // Queen castle.                  
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/2KR3R b kq - 3 21",
                  "e1c1")]
        // Simple move to check an update of en passant move.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/PpP3P1/3P1Q2/8/R3K2R b KkQq c3 0 21",
                  "c2c4")]
        // Capture to check an update of halfmoves.              
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1Q1p/n2B4/1p1NP1pP/Pp4P1/3P4/2P5/R3K2R b KkQq - 0 21",
                  "f3f7")]
        // En passant move.                 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B2P1/1p1NP3/Pp4P1/3P1Q2/2P5/R3K2R b KkQq - 0 21",
                  "h5g6")]
        // Simple move to check an update of en passant move.                 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/Pp1NP1pP/1p4P1/3P1Q2/2P5/R3K2R b KkQq - 0 21",
                  "a4a5")]
        // Queen rook move to check an update of castles.                   
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/1R2K2R b Kkq - 3 21",
                  "a1b1")]
        // King rook move to check an update of castles.                  
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K1R1 b kQq - 3 21",
                  "h1g1")]
        // Capture of king rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "r3k2R/8/8/8/8/8/8/R3K3 b Qq - 0 21",
                  "h1h8")]
        // Capture of queen rook to check an update of castles.                  
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "R3k2r/8/8/8/8/8/8/4K2R b Kk - 0 21",
                  "a1a8")]
        // Pawn promotion to queen 
        [TestCase("K1k2r1r/6P1/8/8/8/8/8/8 w - - 2 20",
                  "K1k2Q1r/8/8/8/8/8/8/8 b - - 0 21",
                  "g7f8q")]
        // Pawn promotion to rook
        [TestCase("K1k2r1r/6P1/8/8/8/8/8/8 w - - 2 20",
                  "K1k2rRr/8/8/8/8/8/8/8 b - - 0 21",
                  "g7g8r")]
        // Pawn promotion to bishop
        [TestCase("K1k2r1r/6P1/8/8/8/8/8/8 w - - 2 20",
                  "K1k2r1B/8/8/8/8/8/8/8 b - - 0 21",
                  "g7h8b")]
        // Pawn promotion to knight
        [TestCase("K1k2r1r/6P1/8/8/8/8/8/8 w - - 2 20",
                  "K1k2r1N/8/8/8/8/8/8/8 b - - 0 21",
                  "g7h8n")]

        // 2. Black moves
        // King castle.       
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r4rk1/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQ - 3 21",
                  "e8g8")]
        // Queen castle.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "2kr3r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQ - 3 21",
                  "e8c8")]
        // En passant move.                                                     
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/6P1/p2P1Q2/2P5/R3K2R w KkQq - 0 21",
                  "b4a3")]
        // Simple move to check an update of en passant move. 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k2r/p2p3p/n2B4/1p1NPppP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq f6 0 21",
                  "f7f5")]
        // King rook move to check an update of castles.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k1r1/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQq - 3 21",
                  "h8g8")]
        // Queen rook move to check an update of castles. 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "1r2k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQ - 3 21",
                  "a8b8")]
        // Capture of king rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "r3k3/8/8/8/8/8/8/R3K2r b Qq - 0 21",
                  "h8h1")]
        // Capture of queen rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "4k2r/8/8/8/8/8/8/r3K2R b Kk - 0 21",
                  "a8a1")]
        // Pawn promotion to queen 
        [TestCase("K1k5/8/8/8/8/8/6p1/5R1R b - - 2 20",
                  "K1k5/8/8/8/8/8/8/5q1R w - - 0 21",
                  "g2f1q")]
        // Pawn promotion to rook
        [TestCase("K1k5/8/8/8/8/8/6p1/5R1R b - - 2 20",
                  "K1k5/8/8/8/8/8/8/5RrR w - - 0 21",
                  "g2g1r")]
        // Pawn promotion to bishop
        [TestCase("K1k5/8/8/8/8/8/6p1/5R1R b - - 2 20",
                  "K1k5/8/8/8/8/8/8/5R1b w - - 0 21",
                  "g2h1b")]
        // Pawn promotion to knight
        [TestCase("K1k5/8/8/8/8/8/6p1/5R1R b - - 2 20",
                  "K1k5/8/8/8/8/8/8/5R1n w - - 0 21",
                  "g2h1n")]
        public void PerformMove(string startGameStateNotation, string endGameStateNotation, string moveNotation)
        {
            // Serialize.
            var startGameState = StandardFENSerializer.DeserializeFromFEN(startGameStateNotation);

            // Perform move.
            var move = StandardFENSerializer.NotationToMove(moveNotation);
            var endGameState = MoveApplier.ApplyMove(startGameState, move);

            // Deserialize.
            var endGameStateFENNotationOutput = StandardFENSerializer.SerializeToFEN(endGameState);

            Assert.That(endGameStateNotation, Is.EqualTo(endGameStateFENNotationOutput));
        }
    }
}
