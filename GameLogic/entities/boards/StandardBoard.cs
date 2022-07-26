using GameLogic.Entities.Pieces;

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

        // Checks if the cell is valid.
        private void EnsureCellIsOnBoard(Cell cell)
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

        // Checks if the cell contains any pieces.
        public bool IsEmpty(Cell cell)
        {
            EnsureCellIsOnBoard(cell);
            return !_positionToPiece.ContainsKey(cell);
        }

        // Find piece by cell.
        public Piece? GetPiece(Cell cell)
        {
            EnsureCellIsOnBoard(cell);
            if (IsEmpty(cell))
            {
                return null;
            }

            return _positionToPiece[cell];
        }

        // Set piece at cell.
        public void SetPiece(Cell cell, Piece piece)
        {
            EnsureCellIsOnBoard(cell);
            RemovePiece(cell);
            _positionToPiece[cell] = piece;
        }

        // Remove the piece from the cell.
        public void RemovePiece(Cell cell)
        {

            EnsureCellIsOnBoard(cell);
            _positionToPiece.Remove(cell);
        }

        // Finds all piece's cells/locations by color and piece type.
        public IEnumerable<Cell> GetCellsWithPieces(Color? filterByColor = null, PieceType? filterByPieceType = null)
        {
            var cells = _positionToPiece.Keys
                        .Where((cell) => filterByColor == null || _positionToPiece[cell].Color == filterByColor)
                        .Where((cell) => filterByPieceType == null || _positionToPiece[cell].Type == filterByPieceType);

            return cells;
        }
    }
}
