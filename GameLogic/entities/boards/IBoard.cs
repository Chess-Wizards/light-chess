using GameLogic.Entities.Pieces;

namespace GameLogic.Entities.Boards
{
    // Represents board functionality.
    public interface IBoard
    {
        // Checks if the cell is on the board.
        bool IsOnBoard(Cell cell);

        // Checks if the cell contains a piece.
        bool IsEmpty(Cell cell);

        // Returns the piece from the cell.
        Piece? GetPiece(Cell cell);

        // Sets the piece on the cell.
        void SetPiece(Cell cell, Piece piece);

        // Removes the piece from the cell.
        void RemovePiece(Cell cell);

        // Returns non-empty cells.
        IEnumerable<Cell> GetCellsWithPieces(Color? filterByColor = null,
                                             PieceType? filterByPieceType = null);

        // Creates a shallow copy of the board.
        StandardBoard Copy(); // TODO: check should it return a StandardBoard?
    }
}
