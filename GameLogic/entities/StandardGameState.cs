using System;
using System.Collections.Generic;


namespace GameLogic
{
    // This class represents the game state. Game state can be uniquely identified by FEN notation.
    // In addition, the + operator is overloaded to get the next game state after move.
    //
    // Please refer to FEN notation (https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation). 
    //
    // TODO: pawn promotion is not implemented
    public class StandardGameState : GameState
    {
        public Color EnemyColor;

        public StandardGameState(
            StandardBoard board,
            Color color,
            List<Castle> avaialbleCastleMoves,
            Cell? enPassantCell,
            int halfmoveNumber,
            int fullmoveNumber
        )
        {
            Board = board;
            ActiveColor = color;
            EnemyColor = color.Change();
            AvaialbleCastleMoves = avaialbleCastleMoves;
            EnPassantCell = enPassantCell;
            HalfmoveNumber = halfmoveNumber;
            FullmoveNumber = fullmoveNumber;
        }

        // Get next game state.
        // 
        // Parameters
        // ----------
        // gameState: The start/initial game state.
        // move: The move to perform. The move must be valid/possible.
        //
        // Returns
        // -------
        // The next game state.
        public static StandardGameState operator +(StandardGameState gameState,
                                                  Move move)
        {
            var piece = (Piece)gameState.Board[move.StartCell];
            var deltaX = move.EndCell.X - move.StartCell.X;
            StandardBoard nextBoard;

            // Castle.
            if (piece.Type == PieceType.King || deltaX >= 2)
            {
                nextBoard = PerformCastle(gameState.Board, move);
            }
            // Enpassant move.
            else if (gameState.EnPassantCell != null
                    && move.EndCell == (Cell)gameState.EnPassantCell)
            {
                nextBoard = PerformEnPassantMove(gameState.Board, move);
            }
            // Simple move.
            else
            {
                nextBoard = PerformMove(gameState.Board, move);
            }

            // Next castles.
            var nextAvaialbleCastleMoves = GetCastlesAfterMove(gameState.Board,
                                                               move,
                                                               gameState.AvaialbleCastleMoves);

            // Next cells. 
            var nextEnPassantCell = GetEnPassantCellAfterMove(gameState.Board, move);

            // Next HalfmoveNumber.
            var movePawn = piece.Type == PieceType.Pawn;
            var moveCapture = !gameState.Board.IsEmpty(move.EndCell) ||
                            move.EndCell == gameState.EnPassantCell;
            var nextHalfmoveNumber = movePawn || moveCapture ? 0 : gameState.HalfmoveNumber + 1;

            // Next FullmoveNumber.
            var nextFullmoveNumber = gameState.FullmoveNumber + 1;

            return new StandardGameState(
                nextBoard,
                gameState.EnemyColor,
                nextAvaialbleCastleMoves,
                nextEnPassantCell,
                nextHalfmoveNumber,
                nextFullmoveNumber
            );
        }

        // Perform castle. The castle must be possible/valid.
        //
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        // 
        // Returns
        // -------
        // The next board with performed move.
        private static StandardBoard PerformCastle(StandardBoard board,
                                                   Move move)
        {
            // Get a new board.
            var nextBoard = board.ShallowCopy();

            var deltaX = move.EndCell.X - move.StartCell.X;
            var y = board[move.StartCell]?.Color == Color.White ? 0 : 7;
            int nextXKing;
            int nextXRook;
            int xRook;

            // Short/king castle.
            if (deltaX > 0)
            {
                nextXKing = 6;
                nextXRook = 5;
                xRook = 7;
            }
            // Long/queen castle.
            else
            {
                nextXKing = 2;
                nextXRook = 3;
                xRook = 0;
            }

            var nextKingCell = new Cell(nextXKing, y);
            var nextRookCell = new Cell(nextXRook,
                                        y);
            var rookCell = new Cell(xRook, y);
            // Perform castle.
            var kingPiece = nextBoard[move.StartCell];
            var rookPiece = nextBoard[rookCell];
            nextBoard[move.StartCell] = null;
            nextBoard[rookCell] = null;
            nextBoard[nextKingCell] = kingPiece;
            nextBoard[nextRookCell] = rookPiece;

            return nextBoard;
        }

        // Perform en passant move. The en passant move must be possible/valid.
        //
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        //
        // Returns
        // -------
        // The next board with performed move.
        private static StandardBoard PerformEnPassantMove(StandardBoard board,
                                                          Move move)
        {
            // Get a new board.
            var nextBoard = board.ShallowCopy();
            var piece = (Piece)board[move.StartCell];

            // Move own pawn.
            nextBoard[move.EndCell] = piece;
            nextBoard[move.StartCell] = null;

            // Capture enemy pawn.
            var enemyCellWithPawn = new Cell(move.EndCell.X, move.StartCell.Y);
            nextBoard[enemyCellWithPawn] = null;

            return nextBoard;
        }

        // Perform the simple move. The move must be possible/valid.
        //
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        //
        // Returns
        // -------
        // The next board with performed move.
        private static StandardBoard PerformMove(StandardBoard board,
                                                 Move move)
        {
            // Get a new board.
            var nextBoard = board.ShallowCopy();
            var piece = (Piece)nextBoard[move.StartCell];

            // Perform move.
            nextBoard[move.EndCell] = piece;
            nextBoard[move.StartCell] = null;

            return nextBoard;
        }

        // Get a list of possible castles after the move is performed.
        //
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        // castles: A list of possible castles at start/initial board.
        //
        // Returns
        // -------
        // A list of possible castles after the move is performed.
        private static List<Castle> GetCastlesAfterMove(StandardBoard board,
                                                        Move move,
                                                        List<Castle> castles)
        {
            var nextCastles = new List<Castle>(castles);
            var piece = (Piece)board[move.StartCell];
            // Castles.
            var kingCastle = new Castle(piece.Color, CastleType.King);
            var queenCastle = new Castle(piece.Color, CastleType.Queen);
            // Enemy castles.
            var kingEnemyCastle = new Castle(piece.Color.Change(), CastleType.King);
            var queenEnemyCastle = new Castle(piece.Color.Change(), CastleType.Queen);
            // Initial rook cells.
            var y = piece.Color == Color.White ? 0 : 7;
            var kingRookInitialCell = new Cell(7, y);
            var queenRookInitialCell = new Cell(0, y);
            // Initial enemy rook cells.
            var yEnemy = piece.Color == Color.White ? 7 : 0;
            var kingEnemyRookInitialCell = new Cell(7, yEnemy);
            var queenEnemyRookInitialCell = new Cell(0, yEnemy);

            // Update castles by removing castles.
            if (piece.Type == PieceType.King)
            {
                nextCastles.Remove(kingCastle);
                nextCastles.Remove(queenCastle);
            }
            else if (move.StartCell == kingRookInitialCell)
            {
                nextCastles.Remove(kingCastle);
            }
            else if (move.StartCell == queenRookInitialCell)
            {
                nextCastles.Remove(queenCastle);
            }

            // Update castles by removing enemy castles.
            if (move.EndCell == kingEnemyRookInitialCell)
            {
                nextCastles.Remove(kingEnemyCastle);
            }
            else if (move.EndCell == queenEnemyRookInitialCell)
            {
                nextCastles.Remove(queenEnemyCastle);
            }

            return nextCastles;
        }

        // Get en passant cell after the move is performed.
        // 
        // Parameters
        // ----------
        // board: The start/initial board.
        // move: The move to perform.
        //
        // Returns
        // -------
        // The en passant cell after the move is performed.
        private static Cell? GetEnPassantCellAfterMove(StandardBoard board,
                                                       Move move)
        {
            var piece = (Piece)board[move.StartCell];
            var deltaY = move.EndCell.Y - move.StartCell.Y;

            // Return en passant cell if the pawn moves forward on two cells.
            if (piece.Type == PieceType.Pawn &&
                Math.Abs(deltaY) == 2)
            {
                var enPassantY = (move.StartCell.Y + move.EndCell.Y) / 2;
                return new Cell(move.StartCell.X, enPassantY);
            }

            return null;
        }
    }
}
