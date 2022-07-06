using System;
using System.Linq;
using System.Collections.Generic;

namespace LightChess
{
    public class StandardBoard: IBoard
    {
        // The class represents the board containing piece's locations/cells. 
        // The width means A-H, while height - 1-8.
        public int Width = 8;
        public int Height = 8;

        // Dictionary to save pieces by cell.
        public Dictionary<Cell, Piece> PositionToPiece;

        public StandardBoard()
        {
            PositionToPiece = new Dictionary<Cell, Piece>();
        }

        public StandardBoard(Dictionary<Cell, Piece> positionToPiece)
        {
            // Shallow copy of dictionary.
            PositionToPiece = new Dictionary<Cell, Piece>(positionToPiece);
        }

        public StandardBoard ShallowCopy()
        {
            // Deep copy of the standard board.
            //
            // Returns
            // -------
            // The board with pieces equal to the current/this board.

            return new StandardBoard(PositionToPiece);
        }

        private void CheckCell(Cell cell)
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

            if (!OnBoard(cell))
            {
                throw new ArgumentOutOfRangeException($"Invalid cell coordinates {cell.X} and {cell.Y}");
            }
        }

        public bool OnBoard(Cell cell)
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

            return 0 <= cell.X &&
                   cell.X < Width &&
                   0 <= cell.Y &&
                   cell.Y < Height;
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

            CheckCell(cell);
            return !PositionToPiece.ContainsKey(cell);
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

            CheckCell(cell);
            if (PositionToPiece.ContainsKey(cell))
                return PositionToPiece[cell];
            return null;
        }

        public void SetPiece(Cell cell, 
                             Piece piece)
        {
            // Set piece at cell.
            //
            // Parameters
            // ----------
            // cell: The cell.

            CheckCell(cell);
            PositionToPiece[cell] = piece;
        }

        public void RemovePiece(Cell cell)
        {
            // Remove the piece from the cell.
            //
            // Parameters
            // ----------
            // cell: The cell.  

            CheckCell(cell);
            PositionToPiece.Remove(cell);
        }

        public Piece? this[Cell cell]
        {
            get 
            {
                return GetPiece(cell);
            }
            set  
            {
                RemovePiece(cell);
                if (value != null)
                    SetPiece(cell, (Piece)value);
            }
        }

        public List<Cell> GetCellsWithPieces(Color? filterByColor=null, 
                                             PieceType? filterByPieceType=null)
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

            var cells = PositionToPiece.Keys
                        .Where((cell) =>  filterByColor == null 
                                          ? true 
                                        : PositionToPiece[cell].Color == filterByColor)
                        .Where((cell) =>  filterByPieceType == null 
                                        ? true 
                                        : PositionToPiece[cell].Type == filterByPieceType)
                        .ToList();

            return cells;
        }
    }
}
