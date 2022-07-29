using NUnit.Framework;
using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.Pieces;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardBoard_Test
    {
        // Create a board and then shallow copy using constructor and method. Finally,
        // check if the change in the initial board influences shallow copies.
        [Test]
        [TestCase(3, 4, Color.White, PieceType.King)]
        public void ShallowCopyPieces(int x, int y, Color pieceColor, PieceType pieceType)
        {
            var board = new StandardBoard();
            var boardShallowCopy = board.Copy();

            var cell = new Cell(x, y);
            var piece = new Piece(pieceColor, pieceType);
            // Set |piece| to |cell| for |board|.
            board.SetPiece(cell, piece);

            Assert.False(board.IsEmpty(cell));
            Assert.True(boardShallowCopy.IsEmpty(cell));
        }

        [Test]
        [TestCase(0, 0, true)]
        [TestCase(0, -1, false)]
        [TestCase(-1, 0, false)]
        [TestCase(7, 7, true)]
        [TestCase(7, 8, false)]
        [TestCase(8, 7, false)]
        public void CheckCellCorrectness(int x, int y, bool cellOnBoardExpected)
        {
            /* Create a cell and check if the cell is on board.
            */

            var board = new StandardBoard();
            var cell = new Cell(x, y);
            Assert.AreEqual(board.IsOnBoard(cell), cellOnBoardExpected);
        }


        [Test]
        [TestCase(3, 4, Color.White, PieceType.King)]
        public void CheckPiecesOnBoard(int x, int y, Color pieceColor, PieceType pieceType)
        {
            /* Set piece on cell and check the correctness with different filtering arguments. 
            */

            var board = new StandardBoard();
            var cell = new Cell(x, y);
            var piece = new Piece(pieceColor, pieceType);
            board.SetPiece(cell, piece);

            CollectionAssert.AreEqual(board.GetCellsWithPieces(), new List<Cell>() { cell });

            // Iterate over colors.
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                // Iterate over piece types.
                foreach (PieceType currentPieceType in Enum.GetValues(typeof(PieceType)))
                {
                    // Get all cells containing piece with |color| color and |pieceType| piece type.
                    var cells = board.GetCellsWithPieces(filterByColor: color,
                                                         filterByPieceType: currentPieceType).ToList();

                    if (color == pieceColor && currentPieceType == pieceType)
                    {
                        CollectionAssert.AreEqual(cells, new List<Cell>() { cell });
                    }
                    else
                    {
                        Assert.IsEmpty(cells);
                    }
                }
            }
        }

        [Test]
        [TestCase(3, 4, Color.White, PieceType.King)]
        public void BoardAction(int x, int y, Color pieceColor, PieceType pieceType)
        {
            /* Check the board functionality, such as setting, removing, and replacing pieces.
            */

            var board = new StandardBoard();
            var cell = new Cell(x, y);
            var piece = new Piece(pieceColor, pieceType);

            // Set piece.
            board.SetPiece(cell, piece);
            Assert.AreEqual(board.GetPiece(cell), piece);
            Assert.That(board.GetCellsWithPieces(), Has.Exactly(1).Items);

            // Remove piece.
            board.RemovePiece(cell);
            Assert.Null(board.GetPiece(cell));
            Assert.IsEmpty(board.GetCellsWithPieces());

            // Replace piece.
            board.SetPiece(cell, piece);
            var pieceNext = new Piece(pieceColor.Change(), pieceType);
            board.SetPiece(cell, pieceNext);
            Assert.AreEqual(board.GetPiece(cell), pieceNext);
            Assert.That(board.GetCellsWithPieces(), Has.Exactly(1).Items);
        }
    }
}
