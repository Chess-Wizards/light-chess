using GameLogic.Entities.Pieces;

namespace GameLogic.Entities.Boards
{
    // Represents the board containing pieces' locations/cells. 
    // The width means A-H, while height - 1-8.
    public class StandardBoard : IRectangularBoard
    {
        // inherited from IRectangularBoard with duplicating the code? why not "readonly" field?
        public int Width { get; }
        public int Height { get; }

        // Dictionary to save pieces by cell.
        private readonly Dictionary<Cell, Piece> _positionToPiece; // maybe _cellToPiece sounds better?

        public StandardBoard()
        {
            Width = StandardBoardConstants.Size; // seems strange
            Height = StandardBoardConstants.Size;
            _positionToPiece = new Dictionary<Cell, Piece>();
        }

        // I insist on using underscore to define private methods
        private StandardBoard(Dictionary<Cell, Piece> positionToPiece) : this() // what does this colon mean?
        {
            // Shallow copy of dictionary.
            _positionToPiece = new Dictionary<Cell, Piece>(positionToPiece);
        }

        // do list public methods first?
        // maybe remove methods description duplication?
        // maybe implement methods in the same order as in the interface?

        // Creates a shallow copy of the board.
        public StandardBoard Copy()
        {
            return new StandardBoard(_positionToPiece);
        }

        // Checks if the cell is valid.
        private void EnsureCellIsOnBoard(Cell cell) // maybe rename to smth like "Exists" or at least "EnsureIsOnBoard"?
        {
            if (!IsOnBoard(cell))
            {
                throw new ArgumentOutOfRangeException($"Invalid cell coordinates {cell.X} and {cell.Y}");
            }
        }

        // how to understand that a method throws or does not throw an exception?

        // Checks if the cell is on board.
        public bool IsOnBoard(Cell cell)
        {
            return 0 <= cell.X && cell.X < Width && // TODO: check
                   0 <= cell.Y && cell.Y < Height;
        }

        // Checks if the cell contains any piece.
        public bool IsEmpty(Cell cell)
        {
            EnsureCellIsOnBoard(cell);
            return !_positionToPiece.ContainsKey(cell);
        }

        // Finds the piece by cell.
        public Piece? GetPiece(Cell cell)
        {
            EnsureCellIsOnBoard(cell);
            if (IsEmpty(cell))
            {
                return null;
            }

            return _positionToPiece[cell];
        }

        // Sets the piece on the cell.
        public void SetPiece(Cell cell, Piece piece)
        {
            EnsureCellIsOnBoard(cell);
            RemovePiece(cell);
            _positionToPiece[cell] = piece;
        }

        // Removes the piece from the cell.
        public void RemovePiece(Cell cell)
        {

            EnsureCellIsOnBoard(cell);
            /*return*/ _positionToPiece.Remove(cell);
        }

        // Finds all pieces' cells/locations by color and piece type.
        public IEnumerable<Cell> GetCellsWithPieces(Color? filterByColor = null, PieceType? filterByPieceType = null)
        {
            return _positionToPiece.Keys
                .Where((cell) => filterByColor == null || _positionToPiece[cell].Color == filterByColor)
                .Where((cell) => filterByPieceType == null || _positionToPiece[cell].Type == filterByPieceType);
        }
    }
}
