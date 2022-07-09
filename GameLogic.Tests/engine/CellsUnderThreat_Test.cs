using System;
using System.Linq;
using System.Collections.Generic;
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
                {"a7", new List<string>(){}},
                {"d7", new List<string>(){}},
                {"f7", new List<string>(){}},
                {"g7", new List<string>(){"e6", "f5", "e8"}},
                {"h7", new List<string>(){}},
                {"a6", new List<string>(){"b8", "c7", "c5", "b4"}},
                {"d6", new List<string>(){"b8", "c7", "e7", "f8", "c5", "b4", "a3"}},
                {"b5", new List<string>(){}},
                {"d5", new List<string>(){"b6", "c7", "e7", "f6", "f4", "e3", "c3", "b4"}},
                {"e5", new List<string>(){}},
                {"h5", new List<string>(){}},
                {"g4", new List<string>(){}},
                {"b3", new List<string>(){"a2", "c2"}},
                {"d3", new List<string>(){}},
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
            var board = SerializeHelper.NotationToBoard(fenBoardNotation);

            // Iterate over width.
            for (int x = 0; x < board.Height; x++)
            {
                // Iterate over height.
                for (int y = 0; y < board.Width; y++)
                {
                    var cell = new Cell(x, y);
                    var cellNotation = SerializeHelper.CellToNotation(cell);

                    // Check if piece produces the correct cells under threat.
                    if (correctCellsUnderThreat.ContainsKey(cellNotation))
                    {
                        var cellsUnderThreat = CellsUnderThreat.GetCellsUnderThreat(cell, board)
                                                   .Select((cell) => SerializeHelper.CellToNotation(cell))
                                                   .OrderBy((notation) => (notation))
                                                   .ToList();
                        var expectedCellsUnderThreat = correctCellsUnderThreat[cellNotation]
                                                   .OrderBy((notation) => (notation))
                                                   .ToList();
                        Assert.That(expectedCellsUnderThreat, Is.EqualTo(cellsUnderThreat));
                    }
                    // Check if the cell is empty.
                    else
                    {
                        Assert.Null(board[cell]);
                    }
                }
            }
        }
    }
}
