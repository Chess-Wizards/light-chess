using GameLogic.Entities;
using GameLogic.Entities.Boards;
using GameLogic.Entities.States;
using GameLogic.Entities.Castles;
using GameLogic.Entities.Pieces;


namespace GameLogic.Engine
{
    public static class StandardFENSerializer
    {
        // The class represents the serialization/deserialization to/from FEN notation.
        // Please refer to the FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation).

        private static Dictionary<char, Piece> _mappingNotationToPiece = new Dictionary<char, Piece>()
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
        private static Dictionary<Piece, char> _mappingPieceToNotation = _mappingNotationToPiece.ToDictionary(x => x.Value, x => x.Key);

        private static Dictionary<char, Castle> _mappingNotationToCastle = new Dictionary<char, Castle>()
            {
                {'K', new Castle(Color.White, CastleType.King)},
                {'Q', new Castle(Color.White, CastleType.Queen)},
                {'k', new Castle(Color.Black, CastleType.King)},
                {'q', new Castle(Color.Black, CastleType.Queen)},
            };
        private static Dictionary<Castle, char> _mappingCastleToNotation = _mappingNotationToCastle.ToDictionary(x => x.Value, x => x.Key);

        private static Dictionary<char, Color> _mappingNotationToColor = new Dictionary<char, Color>()
            {
                {'w', Color.White},
                {'b', Color.Black}
            };

        private static Dictionary<char, PieceType> _mappingNotationToPieceType = new Dictionary<char, PieceType>()
            {
                {'n', PieceType.Knight},
                {'b', PieceType.Bishop},
                {'r', PieceType.Rook},
                {'q', PieceType.Queen}
            };
        public static Dictionary<PieceType, char> _mappingPieceTypeToNotation = _mappingNotationToPieceType.ToDictionary(x => x.Value, x => x.Key);


        // Serialize object to FEN notation.
        public static string SerializeToFEN(IStandardGameState objectToSerialize)
        {
            var splitFenNotation = new string[6]
            {
                BoardToNotation(objectToSerialize.Board),
                ColorToNotation(objectToSerialize.ActiveColor),
                CastleToNotation(objectToSerialize.AvailableCastles),
                CellToNotation(objectToSerialize.EnPassantCell),
                objectToSerialize.HalfmoveNumber.ToString(),
                objectToSerialize.FullmoveNumber.ToString()
            };

            var fenNotation = String.Join(" ", splitFenNotation);
            return fenNotation;
        }

        // Deserialize FEN notation to object.
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

        public static Dictionary<Color, char> mappingColorToNotation = _mappingNotationToColor.ToDictionary(x => x.Value, x => x.Key);

        // Deserializes board FEN notation.
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
                    if (_mappingNotationToPiece.ContainsKey(character))
                    {
                        var cell = new Cell(x, y);
                        board.SetPiece(cell, _mappingNotationToPiece[character]);
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
        public static string BoardToNotation(IRectangularBoard board)
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
                        row.Add(_mappingPieceToNotation[(Piece)board.GetPiece(cell)]);
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
        public static Color NotationToColor(string notation)
        {
            if (notation.Length != 1)
                throw new ArgumentException();
            return _mappingNotationToColor[notation[0]];
        }

        // Serialize color to the FEN notation.
        public static string ColorToNotation(Color color)
        {
            return mappingColorToNotation[color].ToString();
        }

        // Deserialize castle FEN notation.
        public static List<Castle> NotationToCastle(string notation)
        {
            return notation.Where((castle) => (castle != '-'))
                           .Select((castle) => (_mappingNotationToCastle[castle]))
                           .ToList();

        }

        // Serialize castles to the FEN notation.
        public static string CastleToNotation(IEnumerable<Castle> castles)
        {
            var notation = String.Join("",
                        castles.Select((castle) => _mappingCastleToNotation[castle])
                               .ToList()
                       );
            return notation.Length == 0 ? "-" : notation;
        }

        // Deserialize cell FEN notation.
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
        public static string CellToNotation(Cell? cell)
        {
            if (cell != null)
            {
                // Integer to letter char. Example: 0 -> 'a'.
                var x = (char)(((Cell)cell).X + 97);
                // Integer to digit char. Example: 8 -> '7'.
                var y = cell.Value.Y + 1;
                return $"{x}{y}";
            }
            return "-";
        }

        // Serialize move to the notation.
        //
        // The notation follows UCI (Universal Chess Interface {StartCell}-{EndCell}{PieceType or Empty}
        public static string MoveToNotation(Move move)
        {
            var promotionPieceTypeNotation = move.PromotionPieceType == null ? "" : _mappingPieceTypeToNotation[(PieceType)move.PromotionPieceType].ToString();
            return $"{CellToNotation(move.StartCell)}{CellToNotation(move.EndCell)}{promotionPieceTypeNotation}";
        }

        // Deserialize move.
        public static Move NotationToMove(string notation)
        {
            PieceType? pieceType = null;
            var lastIndex = notation.Length - 1;
            // Check if the last character is piece type
            if (_mappingNotationToPieceType.ContainsKey(notation[lastIndex]))
            {
                pieceType = _mappingNotationToPieceType[notation[lastIndex]];
                // Remove piece type from |notation|
                notation = notation.Remove(lastIndex);
            }

            var cells = notation.Chunk(2)
                                .Select(cellNotation => (Cell)NotationToCell(new string(cellNotation)))
                                .ToArray(); ;
            return new Move(cells[0], cells[1], promotionPieceType: pieceType);
        }

        // Deserialize move by passing start and end cells.
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
