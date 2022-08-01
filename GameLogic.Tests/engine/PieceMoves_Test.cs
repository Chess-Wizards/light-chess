using NUnit.Framework;
using GameLogic.Entities;
using GameLogic.Engine;
using GameLogic.Engine.Moves;

namespace GameLogic.Tests
{
    [TestFixture]
    public class PieceMoves_Test
    {
        // Creates board position and per each cell checks if piece produces the correct moves.
        [Test]
        public void MovesAllPieces()
        {
            var correctCells = new Dictionary<string, List<string>>
            {
                {"a8", new List<string>(){"b8"}},
                {"c8", new List<string>(){"b7"}},
                {"e8", new List<string>(){"d8", "e7", "f8"}},
                {"g8", new List<string>(){"e7", "f6", "h6"}},
                {"h8", new List<string>(){}},
                {"a7", new List<string>(){}},
                {"d7", new List<string>(){}},
                {"f7", new List<string>(){"f6", "f5"}},
                {"g7", new List<string>(){"h8r", "h8n", "h8q", "h8b"}},
                {"h7", new List<string>(){"h6"}},
                {"a6", new List<string>(){"b8", "c7", "c5", "b4"}},
                {"d6", new List<string>(){"b8", "c7", "e7", "f8", "c5", "b4", "a3"}},
                {"b5", new List<string>(){"b4"}},
                {"d5", new List<string>(){"b6", "c7", "e7", "f6", "f4", "e3", "c3", "b4"}},
                {"e5", new List<string>(){"e6"}},
                {"h5", new List<string>(){"h6"}},
                {"g4", new List<string>(){"g5"}},
                {"b3", new List<string>(){"a2", "c2", "b2"}},
                {"d3", new List<string>(){"d4"}},
                {"f3", new List<string>(){"f7", "f6", "f5", "f4", "f2", "f1",
                                          "e3", "g3", "h3",
                                          "e4", "g2", "h1"}},
                {"a2", new List<string>(){"b3", "a3", "a4"}},
                {"c2", new List<string>(){"b3", "c3", "c4"}},
                {"e2", new List<string>(){"e3", "d2", "f2", "d1", "e1", "f1"}},
                {"h2", new List<string>(){"h1r", "h1n", "h1q", "h1b"}},
                {"a1", new List<string>(){"a2",
                                          "b2", "c3", "d4", "e5",
                                          "b1", "c1", "d1", "e1", "f1"}},
                {"g1", new List<string>(){"f2", "e3", "d4", "c5", "b6"}}
            };

            var fenBoardNotation = "r1b1k1nr/p2p1pPp/n2B4/1p1NP2P/6P1/1p1P1Q2/P1P1K2p/q5b1";
            var board = StandardFENSerializer.NotationToBoard(fenBoardNotation);

            // Iterate over width.
            for (int x = 0; x < board.Height; x++)
            {
                // Iterate over height.
                for (int y = 0; y < board.Width; y++)
                {
                    var cell = new Cell(x, y);
                    var cellNotation = StandardFENSerializer.CellToNotation(cell);

                    // Check if the piece produces the correct moves.
                    if (correctCells.ContainsKey(cellNotation))
                    {
                        var cells = PieceMoves.GetMoves(cell, board)
                                              .Select((move) => StandardFENSerializer.MoveToNotation(move))
                                              .OrderBy((notation) => (notation))
                                              .ToList();
                        var expectedCells = correctCells[cellNotation].Select(suffixNotation => $"{cellNotation}{suffixNotation}")
                                                                      .OrderBy((notation) => (notation))
                                                                      .ToList();

                        CollectionAssert.AreEqual(cells, expectedCells);
                    }
                    // Check if cell is empty.
                    else
                    {
                        Assert.IsNull(board.GetPiece(cell));
                    }
                }
            }
        }
    }
}
