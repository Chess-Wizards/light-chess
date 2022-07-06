using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    public class StandardBoard: IBoard
    {
        /*
            The class represents the board containing piece's locations/cells. 
            The width means A-H, while height - 1-8.
        */

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

        /// <summary>
        /// Deep copy of the standard board.
        /// </summary>
        /// <returns>
        /// The board with pieces equal to the current/this board. 
        /// </returns>
        public StandardBoard ShallowCopy()
        {
            return new StandardBoard(PositionToPiece);
        }

        /// <summary>
        /// Checks if the cell is valid.
        /// </summary>
        /// <exception 
        ///cref="ArgumentOutOfRangeException">Invalid coordinates.
        ///</exception>
        private void CheckCell(Cell cell)
        {
            if (!OnBoard(cell))
            {
                throw new ArgumentOutOfRangeException($"Invalid cell coordinates {cell.X} and {cell.Y}");
            }
        }

        /// <summary>
        /// Checks if the cell is on board.
        /// </summary>
        /// <param name="cell">
        /// The cell to check.
        /// </param>
        /// <returns>
        /// true if the cell is on board, otherwise false. 
        /// </returns>
        public bool OnBoard(Cell cell)
        {
            return 0 <= cell.X &&
                   cell.X < Width &&
                   0 <= cell.Y &&
                   cell.Y < Height;
        }

        /// <summary>
        /// Checks if the cell contains any pieces.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// true if the cell contains a piece, otherwise false. 
        /// </returns>
        public bool IsEmpty(Cell cell)
        {
            CheckCell(cell);
            return !PositionToPiece.ContainsKey(cell);
        }

        /// <summary>
        /// Find piece by cell.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// piece if exists, otherwise null. 
        /// </returns>
        public Piece? GetPiece(Cell cell)
        {
            CheckCell(cell);
            if (PositionToPiece.ContainsKey(cell))
                return PositionToPiece[cell];
            return null;
        }

        /// <summary>
        /// Set piece at cell.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        public void SetPiece(Cell cell, 
                             Piece piece)
        {
            CheckCell(cell);
            PositionToPiece[cell] = piece;
        }

        /// <summary>
        /// Remove the piece from the cell.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        public void RemovePiece(Cell cell)
        {
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

        /// <summary>
        /// Finds all piece's cells/locations by color and piece type.
        /// </summary>
        /// <param name="filterByColor">
        /// The color to filter. If color is null, then no filtering by color is applied.
        /// </param>
        /// <param name="filterByPieceType">
        /// The piece type to filter. If the piece type is null, then no filtering by type is applied.
        /// </param>
        /// <returns>
        /// A list containing piece's cells/locations. 
        /// </returns>
        public List<Cell> GetCellsWithPieces(Color? filterByColor=null, 
                                             PieceType? filterByPieceType=null)
        {
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
