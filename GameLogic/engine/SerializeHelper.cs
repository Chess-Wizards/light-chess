using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
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
        public static Dictionary<Piece, char> mappingPieceToNotation = mappingNotationToPiece.ToDictionary(x => x.Value, x => x.Key);

        public static Dictionary<char, Castle> mappingNotationToCastle = new Dictionary<char, Castle>()
            {
                {'K', new Castle(Color.White, CastleType.King)},
                {'Q', new Castle(Color.White, CastleType.Queen)},
                {'k', new Castle(Color.Black, CastleType.King)},
                {'q', new Castle(Color.Black, CastleType.Queen)},
            };
        public static Dictionary<Castle, char> mappingCastleToNotation = mappingNotationToCastle.ToDictionary(x => x.Value, x => x.Key);

        public static Dictionary<char, Color> mappingNotationToColor = new Dictionary<char, Color>()
            {
                {'w', Color.White},
                {'b', Color.Black}
            };
        public static Dictionary<Color, char> mappingColorToNotation = mappingNotationToColor.ToDictionary(x => x.Value, x => x.Key);

        // Deserializes board FEN notation.
        //
        // Each rank is described, starting with rank 8 and ending with rank 1, with a "/" between each one; 
        // within each rank, the contents of the squares are described in order from the a-file to the h-file. 
        // Each piece is identified by a single letter taken from the standard English names in algebraic notatio.
        // A set of one or more consecutive empty squares within a rank is denoted by a digit from "1" to "8", 
        // corresponding to the number of squares.
        //
        // Parameters
        // ----------
        // notation: The board FEN notation.
        //
        // Returns
        // -------
        // The deserialized board.
        static public StandardBoard NotationToBoard(string notation)
        {
            var board = new StandardBoard();

            var rows = notation.Split('/');
            int y = rows.Count() - 1;
            // Iterate over height from up (7) to bottom (0). 
            foreach (var row in rows)
            {
                var x = 0;
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

        // Serialize board to FEN notation.
        //
        // Each rank is described, starting with rank 8 and ending with rank 1, with a "/" between each one; 
        // within each rank, the contents of the squares are described in order from the a-file to the h-file. 
        // Each piece is identified by a single letter taken from the standard English names in algebraic notatio.
        // A set of one or more consecutive empty squares within a rank is denoted by a digit from "1" to "8", 
        // corresponding to the number of squares.
        //
        // Parameters
        // ----------
        // board: The board to serialize.
        //
        // Returns
        // -------
        // The serialized board.
        static public string BoardToNotation(StandardBoard board)
        {
            var rows = new List<string>();

            // Iterate over height from up (7) to bottom (0).
            for (int y = board.Height - 1; y > -1; y--)
            {
                var row = new List<char>();
                int numberEmptyCells = 0;
                // Iterate over width from left (a) to right (h).
                for (int x = 0; x < board.Width; x++)
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
                        if (numberEmptyCells != 0) row.Add(Convert.ToChar(numberEmptyCells + 48));
                        row.Add(mappingPieceToNotation[(Piece)board[cell]]);
                        numberEmptyCells = 0;
                    }
                }
                // Add the most right |numberEmptyCells| (if not zero) cells to |row|
                if (numberEmptyCells != 0) row.Add(Convert.ToChar(numberEmptyCells + 48));

                rows.Add(String.Join("", row));
            }

            // Join rows into notation. 
            var notation = String.Join("/", rows);

            return notation;
        }

        // Deserialize color FEN notation.
        //
        // "w" means that White is to move; "b" means that Black is to move.
        //
        // Parameters
        // ----------
        // notation: The notation to deserialize.
        //
        // Returns
        // -------
        // The deserialized color.
        static public Color NotationToColor(string notation)
        {
            if (notation.Length != 1)
                throw new ArgumentException();
            return mappingNotationToColor[notation[0]];
        }

        // Serialize color to the FEN notation.
        //
        // "w" means that White is to move; "b" means that Black is to move.
        //
        // Parameters
        // ----------
        // color: The color to serialize.
        //
        // Returns
        // -------
        // The serialized color.
        static public string ColorToNotation(Color color)
        {
            return mappingColorToNotation[color].ToString();
        }

        // Deserialize castle FEN notation.
        //
        // If neither side has the ability to castle, this field uses the character "-". 
        // Otherwise, this field contains one or more letters: "K" if White can castle kingside, 
        // "Q" if White can castle queenside, "k" if Black can castle kingside, and "q" if Black can castle queenside. 
        // A situation that temporarily prevents castling does not prevent the use of this notation.
        //
        // Parameters
        // ----------
        // notation: The notation to deserialize.
        //
        // Returns
        // -------
        // The deserialized castles.
        static public List<Castle> NotationToCastle(string notation)
        {
            return notation.Where((castle) => (castle != '-'))
                           .Select((castle) => (mappingNotationToCastle[castle]))
                           .ToList();

        }

        // Serialize castles to the FEN notation.
        //
        // If neither side has the ability to castle, this field uses the character "-". 
        // Otherwise, this field contains one or more letters: "K" if White can castle kingside, 
        // "Q" if White can castle queenside, "k" if Black can castle kingside, and "q" if Black can castle queenside. 
        // A situation that temporarily prevents castling does not prevent the use of this notation.
        //
        // Parameters
        // ----------
        // castles: A list of castles.
        //
        // Returns
        // -------
        // The serialized castles.
        static public string CastleToNotation(List<Castle> castles)
        {
            var notation = String.Join("",
                        castles.Select((castle) => mappingCastleToNotation[castle])
                               .ToList()
                       );
            return notation.Length == 0 ? "-" : notation;
        }

        // Deserialize cell FEN notation.
        //
        // Parameters
        // ----------
        // notation: The notation to deserialize or '-'.
        //
        // Exceptions
        // ----------
        // ArgumentException: The length of notation is not equal to two.
        //
        // Returns
        // -------
        // The deserialized cell or null.
        static public Cell? NotationToCell(string notation)
        {
            if (notation != "-")
            {
                if (notation.Length != 2)
                    throw new ArgumentException("Invalid length of the cell notation");
                // Char letter to integer. Example: 'a' -> 0.
                var x = (int)notation[0] - 97;
                // Char digit to integer. Example: '8' -> 7.
                var y = Int32.Parse(notation[1].ToString()) - 1;
                return new Cell(x, y);
            }
            return null;
        }

        // Serialize cell to the FEN notation.
        //
        // Parameters
        // ----------
        // cell: The cell to serialize or null.
        //
        // Returns
        // -------
        // The serialized cell or '-'. 
        static public string CellToNotation(Cell? cell)
        {
            if (cell != null)
            {
                // Integer to letter char. Example: 0 -> 'a'.
                var x = (char)(((Cell)cell).X + 97);
                // Integer to digit char. Example: 8 -> '7'.
                var y = ((Cell)cell).Y + 1;
                return $"{x}{y}";
            }
            return "-";
        }
    }
}
