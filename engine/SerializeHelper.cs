using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace LightChess
{

    public static class SerializeHelper
    {
        public static Dictionary<char, Piece> mappingNotationToPiece = new Dictionary<char, Piece>()
            {
                {'P', new Piece(Color.White, PieceType.Pawn)},
                {'N', new Piece(Color.White, PieceType.Knight)},
                {'B', new Piece(Color.White, PieceType.Bishop)},
                {'R', new Piece(Color.White, PieceType.Rook)},
                {'Q', new Piece(Color.White, PieceType.Queen)},
                {'K', new Piece(Color.White, PieceType.King)},
                {'p', new Piece(Color.Black, PieceType.Pawn)},
                {'n', new Piece(Color.Black, PieceType.Knight)},
                {'b', new Piece(Color.Black, PieceType.Bishop)},
                {'r', new Piece(Color.Black, PieceType.Rook)},
                {'q', new Piece(Color.Black, PieceType.Queen)},
                {'k', new Piece(Color.Black, PieceType.King)}
            };
        public static Dictionary<Piece, char> mappingPieceToNotation = mappingNotationToPiece.ToDictionary(x => x.Value, x=> x.Key);

        public static Dictionary<char, Castle> mappingNotationToCastle = new Dictionary<char, Castle>()
            {
                {'K', new Castle(Color.White, CastleType.King)},
                {'Q', new Castle(Color.White, CastleType.Queen)}, 
                {'k', new Castle(Color.Black, CastleType.King)},
                {'q', new Castle(Color.Black, CastleType.Queen)},              
            };
        public static Dictionary<Castle, char> mappingCastleToNotation = mappingNotationToCastle.ToDictionary(x => x.Value, x=> x.Key);

        public static Dictionary<char, Color> mappingNotationToColor = new Dictionary<char, Color>()
            {
                {'w', Color.White},
                {'b', Color.Black}           
            };
        public static Dictionary<Color, char> mappingColorToNotation = mappingNotationToColor.ToDictionary(x => x.Value, x=> x.Key);

        /// <summary>
        /// Deserializes board FEN notation.
        /// </summary>
        /// <param name="notation">
        /// The board FEN notation.
        /// Each rank is described, starting with rank 8 and ending with rank 1, with a "/" between each one; 
        /// within each rank, the contents of the squares are described in order from the a-file to the h-file. 
        /// Each piece is identified by a single letter taken from the standard English names in algebraic notatio.
        /// A set of one or more consecutive empty squares within a rank is denoted by a digit from "1" to "8", 
        /// corresponding to the number of squares.
        /// </param>
        /// <returns>
        /// The deserialized board.
        /// </returns>
        static public StandardBoard NotationToBoard(string notation)
        {
            
            var board = new StandardBoard();

            var rows = notation.Split('/');
            int y = rows.Count() - 1;
            // Iterate over height from up (7) to bottom (0). 
            foreach (var row in rows)
            {
                var x=0;
                // Iterate over width from left (a) to right (h).
                foreach (var character in row)
                {
                    // Set piece to board and increment |x| by 1 .
                    if (mappingNotationToPiece.ContainsKey(character))
                    {
                        var cell = new Cell(x, y);
                        board[cell] = mappingNotationToPiece[character];
                        x += 1;
                    } 
                    // Increment |x| by number of empty cells.
                    else
                    {
                        var numberEmptyCells = Int32.Parse(character.ToString());
                        x += numberEmptyCells;
                    }
                }
                y -= 1;
            }
            return board;
        }

        /// <summary>
        /// Serialize board to FEN notation.
        /// </summary>
        /// <param name="board">
        /// The board to serialize.
        /// </param>
        /// <returns>
        /// The serialized board.
        /// Each rank is described, starting with rank 8 and ending with rank 1, with a "/" between each one; 
        /// within each rank, the contents of the squares are described in order from the a-file to the h-file. 
        /// Each piece is identified by a single letter taken from the standard English names in algebraic notatio.
        /// A set of one or more consecutive empty squares within a rank is denoted by a digit from "1" to "8", 
        /// corresponding to the number of squares.
        /// </returns>
        static public string BoardToNotation(StandardBoard board)
        {
            var rows = new List<string>();

            // Iterate over height from up (7) to bottom (0).
            for (int y=board.Height-1;y>-1;y--)
            {
                var row = new List<char>();
                int numberEmptyCells = 0;
                // Iterate over width from left (a) to right (h).
                for(int x=0; x<board.Width;x++)
                {
                    var cell = new Cell(x, y);
                    // Increment |numberEmptyCells| by 1.
                    if (board.IsEmpty(cell))
                    {
                        numberEmptyCells += 1;   
                    }
                    // Add |numberEmptyCells| (if not zero) and piece to notation.
                    // Set |numberEmptyCells| to zero.
                    else
                    {
                        if (numberEmptyCells != 0) row.Add(Convert.ToChar(numberEmptyCells+48));
                        row.Add(mappingPieceToNotation[(Piece)board[cell]]);
                        numberEmptyCells = 0;
                    }
                }
                // Add the most right |numberEmptyCells| (if not zero) cells to |row|
                if (numberEmptyCells != 0) row.Add(Convert.ToChar(numberEmptyCells+48));
 
                rows.Add(String.Join("", row));
            }

            // Join rows into notation. 
            var notation = String.Join("/", rows);

            return notation;
        }

        /// <summary>
        /// Deserialize color FEN notation.
        /// "w" means that White is to move; "b" means that Black is to move.
        /// </summary>
        /// <param name="notation">
        /// The notation to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized color. 
        /// </returns>
        /// <exception 
        ///cref="ArgumentException">The length of notation is not equal to one.
        ///</exception>
        static public Color NotationToColor(string notation)
        {
            if (notation.Length != 1)
                throw new ArgumentException();
            return mappingNotationToColor[notation[0]];
        }

        /// <summary>
        /// Serialize color to the FEN notation.
        /// </summary>
        /// <param name="color">
        /// The color to serialize.
        /// </param>
        /// <returns>
        /// The serialized color. 
        /// "w" means that White is to move; "b" means that Black is to move.
        /// </returns>
        static public string ColorToNotation(Color color)
        {
            return mappingColorToNotation[color].ToString();
        }

        /// <summary>
        /// Deserialize castle FEN notation.
        /// </summary>
        /// <param name="notation">
        /// The notation to deserialize.
        /// If neither side has the ability to castle, this field uses the character "-". 
        /// Otherwise, this field contains one or more letters: "K" if White can castle kingside, 
        /// "Q" if White can castle queenside, "k" if Black can castle kingside, and "q" if Black can castle queenside. 
        /// A situation that temporarily prevents castling does not prevent the use of this notation.
        /// </param>
        /// <returns>
        /// The deserialized castles. 
        /// </returns>
        static public List<Castle> NotationToCastle(string notation)
        {
            return notation.Where((castle) => (castle != '-'))
                           .Select((castle) => (mappingNotationToCastle[castle]))
                           .ToList();
            
        }

        /// <summary>
        /// Serialize castles to the FEN notation.
        /// </summary>
        /// <param name="castles">
        /// The color to serialize.
        /// If neither side has the ability to castle, this field uses the character "-". 
        /// Otherwise, this field contains one or more letters: "K" if White can castle kingside, 
        /// "Q" if White can castle queenside, "k" if Black can castle kingside, and "q" if Black can castle queenside. 
        /// A situation that temporarily prevents castling does not prevent the use of this notation.
        /// </param>
        /// <returns>
        /// The serialized castles. 
        /// </returns>
        static public string CastleToNotation(List<Castle> castles)
        {
            var notation = String.Join("", 
                        castles.Select((castle) => mappingCastleToNotation[castle])
                               .ToList()
                       );
            return notation.Length == 0 ? "-" : notation;
        }

        /// <summary>
        /// Deserialize cell FEN notation.
        /// </summary>
        /// <param name="notation">
        /// The notation to deserialize or null.
        /// </param>
        /// <returns>
        /// The deserialized cell or "-". 
        /// </returns>
        /// <exception 
        ///cref="ArgumentException">The length of notation is not equal to two.
        ///</exception>
        static public Cell? NotationToCell(string notation)
        {
       
            if (notation != "-")
            {
                if (notation.Length != 2)
                    throw new ArgumentException("Invalid length of the cell notation");
                // Char letter to integer. Example: 'a' -> 0.
                var x = (int)notation[0]-97;
                // Char digit to integer. Example: '8' -> 7.
                var y = Int32.Parse(notation[1].ToString())-1;
                return new Cell(x, y);
            }
            return null;
        }

        /// <summary>
        /// Serialize cell to the FEN notation.
        /// </summary>
        /// <param name="castles">
        /// The cell to serialize or null.
        /// </param>
        /// <returns>
        /// The serialized cell or '-'. 
        /// </returns>
        static public string CellToNotation(Cell? cell)
        {
            if (cell != null)
            {
                // Integer to letter char. Example: 0 -> 'a'.
                var x = (char)(((Cell)cell).X+97);
                // Integer to digit char. Example: 8 -> '7'.
                var y = ((Cell)cell).Y+1;
                return $"{x}{y}";
            }
            return "-";
        }
    }
}
