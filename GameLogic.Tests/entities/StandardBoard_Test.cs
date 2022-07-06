using System;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class StandardBoard_Test
    {
        [Test]
        public void ShallowCopyPieces()
        {
             /* Create a board and then shallow copy using constructor and method. Finally,
             check if the change in the initial board influences shallow copies.
             */

            var board = new StandardBoard();
            var boardShallowCopyConstructor = new StandardBoard(board.PositionToPiece);
            var boardShallowCopy = board.ShallowCopy();

            var cell = new Cell(4, 4);
            var piece = new Piece(Color.White, PieceType.Rook);
            // Set |piece| to |cell| for |board|.
            board[cell] = piece;

            Assert.False(board.IsEmpty(cell));
            Assert.True(boardShallowCopyConstructor.IsEmpty(cell));
            Assert.True(boardShallowCopy.IsEmpty(cell));
        }

        [Test]
        [TestCase(0, 0, true)]
        [TestCase(0, -1, false)]
        [TestCase(-1, 0, false)]
        [TestCase(7, 7, true)]
        [TestCase(7, 8, false)]
        [TestCase(8, 7, false)]
        public void CheckCellCorrectness(int x, int y, bool cellOnBoard)
        {
            /* Create a cell and check if the cell is on board.
            */

            var board = new StandardBoard();
            var cell = new Cell(x, y);
            Assert.AreEqual(board.OnBoard(cell), cellOnBoard);
        }
    

        [Test]
        public void CheckPiecesOnBoard()
        {
            /* Set piece on cell and check the correctness with different filtering arguments. 
            */

            var board = new StandardBoard();
            var cell = new Cell(3, 4);
            var piece = new Piece(Color.White, PieceType.King);
            board[cell] = piece;

            Assert.AreEqual(board.GetCellsWithPieces().Count, 1);
            Assert.AreEqual(board.GetCellsWithPieces()[0], cell);

            // Iterate over colors.
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                // Iterate over piece types.
                foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
                {
                    // Get all cells containing piece with |color| color and |pieceType| piece type.
                    var cells = board.GetCellsWithPieces(filterByColor:color,
                                                         filterByPieceType:pieceType);
                    if (color == Color.White 
                        && pieceType == PieceType.King)
                    {
                        Assert.AreEqual(cells.Count, 1);
                        Assert.AreEqual(cells[0], cell);   
                    }
                    else
                    {
                        Assert.AreEqual(cells.Count, 0);
                    }
                }
            }
        }

        [Test]
        public void BoardAction()
        {
             /* Check the board functionality, such as setting, removing, and replacing pieces.
             */

            var board = new StandardBoard();
            var cell = new Cell(4, 4);
            var piece = new Piece(Color.White, PieceType.Rook);

            // Set piece.
            board[cell] = piece;
            Assert.AreEqual(board[cell], piece);
            Assert.AreEqual(board.GetCellsWithPieces().Count, 1);

            // Remove piece.
            board[cell] = null;
            Assert.AreEqual(board[cell], null);
            Assert.AreEqual(board.GetCellsWithPieces().Count, 0);

            // Replace piece.
            board[cell] = piece;
            var pieceNext = new Piece(Color.Black, PieceType.Bishop);
            board[cell] = pieceNext;
            Assert.AreEqual(board[cell], pieceNext);
            Assert.AreEqual(board.GetCellsWithPieces().Count, 1);
        }
    }
}
