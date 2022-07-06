using System;
using System.Collections.Generic;

namespace GameLogic
{
    public interface IBoard
    {
        // The interface represents the required board functionality.  

        // The method checks if cell coordinates are valid.
        bool OnBoard(Cell cell);

        // The method checks if the cell contains a piece.
        bool IsEmpty(Cell cell);

        // The method returns the piece at the cell.
        Piece? GetPiece(Cell cell);

        // The method sets the piece at the cell.
        void SetPiece(Cell cell,
                      Piece piece);

        // The method removes the piece from the cell.
        void RemovePiece(Cell cell);

        // Get cells containing pieces.
        List<Cell> GetCellsWithPieces(Color? filterByColor = null,
                                      PieceType? filterByPieceType = null);
    }
}
