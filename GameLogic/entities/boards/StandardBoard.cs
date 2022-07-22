namespace GameLogic.Entities.Boards
{
    // Represents the board containing piece's locations/cells. 
    // The width means A-H, while height - 1-8.
    public class StandardBoard : IRectangularBoard
    {

        private static readonly StandardBoardConstants _StandardBoardConstants = new();

        public int Width { get; }
        public int Height { get; }

        // Dictionary to save pieces by cell.
        private readonly Dictionary<Cell, Piece> _positionToPiece;

        public StandardBoard()
        {
            Width = _StandardBoardConstants.Size;
            Height = _StandardBoardConstants.Size;
            _positionToPiece = new Dictionary<Cell, Piece>();
        }

        private StandardBoard(Dictionary<Cell, Piece> positionToPiece) : this()
        {
            // Shallow copy of dictionary.
            _positionToPiece = new Dictionary<Cell, Piece>(positionToPiece);
        }

        // Creates a shallow copy of the board.
        public StandardBoard Copy()
        {
            return new StandardBoard(_positionToPiece);
        }

        private void EnsureCellIsOnBoard(Cell cell)
        {
            // Checks if the cell is valid.
            //
            // Parameters
            // ----------
            // cell: The cell.
            //
            // Exceptions
            // ----------
            // ArgumentOutOfRangeException: Invalid coordinates.

            if (!IsOnBoard(cell))
            {
                throw new ArgumentOutOfRangeException($"Invalid cell coordinates {cell.X} and {cell.Y}");
            }
        }

        public bool IsOnBoard(Cell cell)
        {
            // Checks if the cell is on board.
            //
            // Parameters
            // ----------
            // cell: The cell to check.
            //
            // Returns
            // -------
            // true if the cell is on board, otherwise false.

            return 0 <= cell.X && cell.X < Width &&
                   0 <= cell.Y && cell.Y < Height;
        }

        public bool IsEmpty(Cell cell)
        {
            // Checks if the cell contains any pieces.
            //
            // Parameters
            // ----------
            // cell: The cell to check.
            //
            // Returns
            // -------
            // true if the cell contains a piece, otherwise false.

            EnsureCellIsOnBoard(cell);
            return !_positionToPiece.ContainsKey(cell);
        }

        public Piece? GetPiece(Cell cell)
        {
            // Find piece by cell.
            //
            // Parameters
            // ----------
            // cell: The cell.
            //
            // Returns
            // -------
            // piece if exists, otherwise null. 

            EnsureCellIsOnBoard(cell);
            if (IsEmpty(cell))
            {
                return null;
            }

            return _positionToPiece[cell];
        }

        public void SetPiece(Cell cell, Piece piece)
        {
            // Set piece at cell.
            //
            // Parameters
            // ----------
            // cell: The cell.

            EnsureCellIsOnBoard(cell);
            RemovePiece(cell);
            _positionToPiece[cell] = (Piece)piece; ;
        }

        public void RemovePiece(Cell cell)
        {
            // Remove the piece from the cell.
            //
            // Parameters
            // ----------
            // cell: The cell.  

            EnsureCellIsOnBoard(cell);
            _positionToPiece.Remove(cell);
        }

        public IEnumerable<Cell> GetCellsWithPieces(Color? filterByColor = null,
                                                    PieceType? filterByPieceType = null)
        {
            // Finds all piece's cells/locations by color and piece type.
            //
            // Parameters
            // ----------   
            // filterByColor: The color to filter. If color is null, then no filtering by color is applied.
            // filterByPieceType: The piece type to filter. If the piece type is null, then no filtering by type is applied.
            //
            // Returns
            // -------
            // A list containing piece's cells/locations.

            var cells = _positionToPiece.Keys
                        .Where((cell) => filterByColor == null
                                          ? true
                                        : _positionToPiece[cell].Color == filterByColor)
                        .Where((cell) => filterByPieceType == null
                                        ? true
                                        : _positionToPiece[cell].Type == filterByPieceType)
                        .ToList();

            return cells;
        }
    }
}
