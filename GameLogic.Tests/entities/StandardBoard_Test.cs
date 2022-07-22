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
        public void ShallowCopyPieces()
        {
            var board = new StandardBoard();
            var boardShallowCopy = board.Copy();

            var cell = new Cell(4, 4);
            var piece = new Piece(Color.White, PieceType.Rook);
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
        public void CheckCellCorrectness(int x, int y, bool cellOnBoard)
        {
            /* Create a cell and check if the cell is on board.
            */

            var board = new StandardBoard();
            var cell = new Cell(x, y);
            Assert.That(cellOnBoard, Is.EqualTo(board.IsOnBoard(cell)));
        }


        [Test]
        public void CheckPiecesOnBoard()
        {
            /* Set piece on cell and check the correctness with different filtering arguments. 
            */

            var board = new StandardBoard();
            var cell = new Cell(3, 4);
            var piece = new Piece(Color.White, PieceType.King);
            board.SetPiece(cell, piece);

            Assert.That(board.GetCellsWithPieces().Count, Is.EqualTo(1));
            Assert.That(cell, Is.EqualTo(board.GetCellsWithPieces().ToList()[0]));

            // Iterate over colors.
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                // Iterate over piece types.
                foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
                {
                    // Get all cells containing piece with |color| color and |pieceType| piece type.
                    var cells = board.GetCellsWithPieces(filterByColor: color,
                                                         filterByPieceType: pieceType);
                    if (color == Color.White
                        && pieceType == PieceType.King)
                    {
                        Assert.That(cells.Count, Is.EqualTo(1));
                        Assert.That(cell, Is.EqualTo(cells.ToList()[0]));
                    }
                    else
                    {
                        Assert.That(cells.Count, Is.EqualTo(0));
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
            board.SetPiece(cell, piece);
            Assert.That(piece, Is.EqualTo(board.GetPiece(cell)));
            Assert.That(board.GetCellsWithPieces().Count, Is.EqualTo(1));

            // Remove piece.
            board.RemovePiece(cell);
            Assert.Null(board.GetPiece(cell));
            Assert.That(board.GetCellsWithPieces().Count, Is.EqualTo(0));

            // Replace piece.
            board.SetPiece(cell, piece);
            var pieceNext = new Piece(Color.Black, PieceType.Bishop);
            board.SetPiece(cell, pieceNext);
            Assert.That(pieceNext, Is.EqualTo(board.GetPiece(cell)));
            Assert.That(board.GetCellsWithPieces().Count, Is.EqualTo(1));
        }
    }
}
