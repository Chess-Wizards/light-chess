using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLogic
{
    public static class StandardFENSerializer
    {
        // The class represents the serialization/deserialization to/from FEN notation.
        // Please refer to the FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation).

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

        public static Dictionary<char, PieceType> mappingNotationToPieceType = new Dictionary<char, PieceType>()
            {
                {'n', PieceType.Knight},
                {'b', PieceType.Bishop},
                {'r', PieceType.Rook},
                {'q', PieceType.Queen}
            };
        public static Dictionary<PieceType, char> mappingPieceTypeToNotation = mappingNotationToPieceType.ToDictionary(x => x.Value, x => x.Key);


        // Serialize object to FEN notation.
        //
        // Parameters
        // ----------
        // objectToSerialize: The object to serializer.
        // 
        // Returns
        // -------
        // The FEN notation.
        public static string SerializeToFEN(StandardGameState objectToSerialize)
        {
            var splitFenNotation = new string[6]
            {
                BoardToNotation(objectToSerialize.Board),
                ColorToNotation(objectToSerialize.ActiveColor),
                CastleToNotation(objectToSerialize.AvaialbleCastleMoves),
                CellToNotation(objectToSerialize.EnPassantCell),
                objectToSerialize.HalfmoveNumber.ToString(),
                objectToSerialize.FullmoveNumber.ToString()
            };

            var fenNotation = String.Join(" ", splitFenNotation);
            return fenNotation;
        }

        // Deserialize FEN notation to object.
        //
        // Parameters
        // ----------
        // fenNotation: The FEN notation.
        //
        // Exceptions
        // ----------
        // ArgumentException: Invalid FEN notation.
        // 
        // Returns
        // -------
        // The game. 
        public static StandardGameState DeserializeFromFEN(string fenNotation)
        {
            var splitFenNotation = fenNotation.Split(' ');

            if (splitFenNotation.Count() != 6)
                throw new ArgumentException("Invalid FEN notation.");

            var gameState = new StandardGameState(
                NotationToBoard(splitFenNotation[0]),
                NotationToColor(splitFenNotation[1]),
                NotationToCastle(splitFenNotation[2]),
                NotationToCell(splitFenNotation[3]),
                Int32.Parse(splitFenNotation[4]),
                Int32.Parse(splitFenNotation[5])
            );

            return gameState;
        }

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
        public static StandardBoard NotationToBoard(string notation)
        {
            var board = new StandardBoard();

            var rows = notation.Split('/');
            int y = rows.Count() - 1;
            // Iterate over ranks from up (7) to bottom (0). 
            foreach (var row in rows)
            {
                var x = 0;
                // Iterate over cells from left (a) to right (h) in rank.
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
        public static string BoardToNotation(StandardBoard board)
        {
            var rows = new List<string>();

            // Iterate over cells from up (7) to bottom (0) in file.
            for (int y = board.Height - 1; y > -1; y--)
            {
                var row = new List<char>();
                int numberEmptyCells = 0;
                // Iterate over cells from left (a) to right (h) in rank.
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
        public static Color NotationToColor(string notation)
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
        public static string ColorToNotation(Color color)
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
        public static List<Castle> NotationToCastle(string notation)
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
        public static string CastleToNotation(List<Castle> castles)
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
        public static Cell? NotationToCell(string notation)
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
        public static string CellToNotation(Cell? cell)
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

        // Serialize move to the notation.
        //
        // Parameters
        // ----------
        // move: The move to serialize.
        //
        // Returns
        // -------
        // The serialized move. The notation follows UCI (Universal Chess Interface 
        // {StartCell}-{EndCell}{PieceType or Empty}
        public static string MoveToNotation(Move move)
        {
            var promotionPieceTypeNotation = move.PromotionPieceType == null ? "" : mappingPieceTypeToNotation[(PieceType)move.PromotionPieceType].ToString();
            return $"{CellToNotation(move.StartCell)}{CellToNotation(move.EndCell)}{promotionPieceTypeNotation}";
        }

        // Deserialize move.
        //
        // Parameters
        // ----------
        // notation: The notation to deserialize. The notation follows UCI (Universal Chess Interface)
        // {StartCell}{EndCell}{PieceType or Empty}
        //
        // Returns
        // -------
        // The deserialized move.
        public static Move NotationToMove(string notation)
        {
            PieceType? pieceType = null;
            var lastIndex = notation.Length - 1;
            // Check if the last character is piece type
            if (mappingNotationToPieceType.ContainsKey(notation[lastIndex]))
            {
                pieceType = mappingNotationToPieceType[notation[lastIndex]];
                // Remove piece type from |notation|
                notation = notation.Remove(lastIndex);
            }

            var cells = notation.Chunk(2)
                                .Select(cellNotation => (Cell)NotationToCell(new string(cellNotation)))
                                .ToArray(); ;
            return new Move(cells[0], cells[1], promotionPieceType: pieceType);
        }

        // Deserialize move by passing start and end cells.
        //
        // Parameters
        // ----------
        // startCellNotation: The notation of the start cell.
        // endCellNotation: The notation of the end cell.
        // pieceTypeNotation: The notation of the piece type or empty.
        //
        // Returns
        // -------
        // The deserialized move.
        public static Move NotationToMove(string startCellNotation,
                                          string endCellNotation,
                                          string pieceTypeNotation = "")
        {
            // The notation follows UCI (Universal Chess Interface)
            var notation = $"{startCellNotation}{endCellNotation}{pieceTypeNotation}";
            return NotationToMove(notation);
        }
    }
}
