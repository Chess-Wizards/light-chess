using GameLogic.Entities.Pieces;

namespace GameLogic.Entities.Boards
{
    // Represents the board containing pieces' locations/cells. 
    // The width means A-H, while height - 1-8.
    public class StandardBoard : IRectangularBoard
    {
        public int Width { get; }
        public int Height { get; }

        // Dictionary to save pieces by cell.
        private readonly Dictionary<Cell, Piece> _cellToPiece;

        public StandardBoard()
        {
            Width = StandardBoardConstants.Size;
            Height = StandardBoardConstants.Size;
            _cellToPiece = new Dictionary<Cell, Piece>();
        }

        private StandardBoard(Dictionary<Cell, Piece> positionToPiece) : this()
        {
            // Shallow copy of dictionary.
            _cellToPiece = new Dictionary<Cell, Piece>(positionToPiece);
        }

        // TODO: maybe do list public methods first?
        // TODO: maybe remove methods description duplication?
        // TODO: maybe implement methods in the same order as in the interface?

        // Creates a shallow copy of the board.
        public StandardBoard Copy()
        {
            return new StandardBoard(_cellToPiece);
        }

        // Checks if the cell is valid.
        // Throws an ArgumentOutOfRangeException if not.
        private void EnsureIsOnBoard(Cell cell)
        {
            if (!IsOnBoard(cell))
            {
                throw new ArgumentOutOfRangeException($"Invalid cell coordinates {cell.X} and {cell.Y}");
            }
        }

        // Checks if the cell is on board.
        public bool IsOnBoard(Cell cell)
        {
            return 0 <= cell.X && cell.X < Width &&
                   0 <= cell.Y && cell.Y < Height;
        }

        // Checks if the cell contains any piece.
        public bool IsEmpty(Cell cell)
        {
            EnsureIsOnBoard(cell);
            return !_cellToPiece.ContainsKey(cell);
        }

        // Finds the piece by cell.
        public Piece? GetPiece(Cell cell)
        {
            EnsureIsOnBoard(cell);
            if (IsEmpty(cell))
            {
                return null;
            }

            return _cellToPiece[cell];
        }

        // Sets the piece on the cell.
        public void SetPiece(Cell cell, Piece piece)
        {
            EnsureIsOnBoard(cell);
            RemovePiece(cell);
            _cellToPiece[cell] = piece;
        }

        // Removes the piece from the cell.
        public void RemovePiece(Cell cell)
        {
            EnsureIsOnBoard(cell);
            _cellToPiece.Remove(cell);
        }

        // Finds all pieces' cells/locations by color and piece type.
        public IEnumerable<Cell> GetCellsWithPieces(Color? filterByColor = null, PieceType? filterByPieceType = null)
        {
            return _cellToPiece.Keys
                .Where((cell) => filterByColor == null || _cellToPiece[cell].Color == filterByColor)
                .Where((cell) => filterByPieceType == null || _cellToPiece[cell].Type == filterByPieceType);
        }
    }
}
