using System;
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
                  "e1", "g1")]
        // Queen castle.                  
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/2KR3R b kq - 3 21",
                  "e1", "c1")]
        // Simple move to check an update of en passant move.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/PpP3P1/3P1Q2/8/R3K2R b KkQq c3 0 21",
                  "c2", "c4")]
        // Capture to check an update of halfmoves.              
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1Q1p/n2B4/1p1NP1pP/Pp4P1/3P4/2P5/R3K2R b KkQq - 0 21",
                  "f3", "f7")]
        // En passant move.                 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B2P1/1p1NP3/Pp4P1/3P1Q2/2P5/R3K2R b KkQq - 0 21",
                  "h5", "g6")]
        // Simple move to check an update of en passant move.                 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/Pp1NP1pP/1p4P1/3P1Q2/2P5/R3K2R b KkQq - 0 21",
                  "a4", "a5")]
        // Queen rook move to check an update of castles.                   
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/1R2K2R b Kkq - 3 21",
                  "a1", "b1")]
        // King rook move to check an update of castles.                  
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq g6 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K1R1 b kQq - 3 21",
                  "h1", "g1")]
        // Capture of king rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "r3k2R/8/8/8/8/8/8/R3K3 b Qq - 0 21",
                  "h1", "h8")]
        // Capture of queen rook to check an update of castles.                  
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "R3k2r/8/8/8/8/8/8/4K2R b Kk - 0 21",
                  "a1", "a8")]

        // 2. Black moves
        // King castle.       
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r4rk1/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQ - 3 21",
                  "e8", "g8")]
        // Queen castle.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "2kr3r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQ - 3 21",
                  "e8", "c8")]
        // En passant move.                                                     
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k2r/p2p1p1p/n2B4/1p1NP1pP/6P1/p2P1Q2/2P5/R3K2R w KkQq - 0 21",
                  "b4", "a3")]
        // Simple move to check an update of en passant move. 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k2r/p2p3p/n2B4/1p1NPppP/Pp4P1/3P1Q2/2P5/R3K2R w KkQq f6 0 21",
                  "f7", "f5")]
        // King rook move to check an update of castles.
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "r3k1r1/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KQq - 3 21",
                  "h8", "g8")]
        // Queen rook move to check an update of castles. 
        [TestCase("r3k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R b KkQq a3 2 20",
                  "1r2k2r/p2p1p1p/n2B4/1p1NP1pP/Pp4P1/3P1Q2/2P5/R3K2R w KkQ - 3 21",
                  "a8", "b8")]
        // Capture of king rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "r3k3/8/8/8/8/8/8/R3K2r b Qq - 0 21",
                  "h8", "h1")]
        // Capture of queen rook to check an update of castles.
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KkQq - 2 20",
                  "4k2r/8/8/8/8/8/8/r3K2R b Kk - 0 21",
                  "a8", "a1")]
        public void PerformMove(string startGameStateNotation,
                                string endGameStateNotation,
                                string startCell,
                                string endCell)
        {
            // Serialize.
            var startGameState = StandardFENSerializer.DeserializeFromFEN(startGameStateNotation);

            // Perform move.
            var move = new Move((Cell)StandardFENSerializer.NotationToCell(startCell),
                                (Cell)StandardFENSerializer.NotationToCell(endCell));
            var endGameState = MoveApplier.GetNextGameState(startGameState, move);

            // Deserialize.
            var gameStateFENNotationOutput = StandardFENSerializer.SerializeToFEN(endGameState);

            Assert.That(gameStateFENNotationOutput, Is.EqualTo(endGameStateNotation));
        }
    }
}
