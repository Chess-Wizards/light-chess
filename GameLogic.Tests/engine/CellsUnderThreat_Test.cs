using GameLogic.Engine;
using GameLogic.Engine.Moves;
using GameLogic.Entities;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class CellsUnderThreat_Test
    {
        // Creates board position and per each cell checks if piece produces the correct cells under threat.
        [Test]
        public void CellsUnderThreatAllPieces()
        {
            var correctCellsUnderThreat = new Dictionary<string, List<string>>
            {
                {"a8", new List<string>(){"b8"}},
                {"c8", new List<string>(){"b7"}},
                {"e8", new List<string>(){"d8", "e7", "f8"}},
                {"g8", new List<string>(){"e7", "f6", "h6"}},
                {"h8", new List<string>(){}},
                {"a7", new List<string>(){"b6"}},
                {"d7", new List<string>(){"c6", "e6"}},
                {"f7", new List<string>(){"e6", "g6"}},
                {"g7", new List<string>(){"e6", "f5", "e8"}},
                {"h7", new List<string>(){"g6"}},
                {"a6", new List<string>(){"b8", "c7", "c5", "b4"}},
                {"d6", new List<string>(){"b8", "c7", "e7", "f8", "c5", "b4", "a3"}},
                {"b5", new List<string>(){"a4", "c4"}},
                {"d5", new List<string>(){"b6", "c7", "e7", "f6", "f4", "e3", "c3", "b4"}},
                {"e5", new List<string>(){"f6"}},
                {"h5", new List<string>(){"g6"}},
                {"g4", new List<string>(){"f5"}},
                {"b3", new List<string>(){"a2", "c2"}},
                {"d3", new List<string>(){"c4", "e4"}},
                {"f3", new List<string>(){"f7", "f6", "f5", "f4", "f2", "f1",
                                          "e3", "g3", "h3",
                                          "e4", "g2", "h1"}},
                {"a2", new List<string>(){"b3"}},
                {"c2", new List<string>(){"b3"}},
                {"e2", new List<string>(){"e3", "d2", "f2", "d1", "e1", "f1"}},
                {"a1", new List<string>(){"a2",
                                          "b2", "c3", "d4", "e5",
                                          "b1", "c1", "d1", "e1", "f1"}},
                {"g1", new List<string>(){"f2", "e3", "d4", "c5", "b6", "h2"}}
            };

            var fenBoardNotation = "r1b1k1nr/p2p1pNp/n2B4/1p1NP2P/6P1/1p1P1Q2/P1P1K3/q5b1";
            var board = StandardFENSerializer.NotationToBoard(fenBoardNotation);

            // Iterate over width.
            for (int x = 0; x < board.Height; x++)
            {
                // Iterate over height.
                for (int y = 0; y < board.Width; y++)
                {
                    var cell = new Cell(x, y);
                    var cellNotation = StandardFENSerializer.CellToNotation(cell);

                    // Check if piece produces the correct cells under threat.
                    if (correctCellsUnderThreat.ContainsKey(cellNotation))
                    {
                        var cellsUnderThreat = CellsUnderThreat.GetCellsUnderThreat(cell, board)
                                                               .Select((cell) => StandardFENSerializer.CellToNotation(cell))
                                                               .OrderBy((notation) => (notation))
                                                               .ToList();
                        var expectedCellsUnderThreat = correctCellsUnderThreat[cellNotation]
                                                   .OrderBy((notation) => (notation))
                                                   .ToList();
                        CollectionAssert.AreEqual(cellsUnderThreat, expectedCellsUnderThreat);
                    }
                    // Check if the cell is empty.
                    else
                    {
                        Assert.Null(board.GetPiece(cell));
                    }
                }
            }
        }
    }
}
