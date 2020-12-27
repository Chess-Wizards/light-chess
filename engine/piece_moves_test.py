#!/usr/bin/python3

import unittest
from entities.board import Board
from entities.position import Position
from entities.pieces import Piece
from entities.pieces import PieceType
from entities.colour import Colour
from entities.move import Move
from engine.piece_moves import PieceMoves
from engine.game import Game


class TestPieceMoves(unittest.TestCase):
    """Test of PieceMoves class.
        Create dummy board by means of setUp() method and check the correctness of pieces moves.
    """

    def setUp(self) -> None:
        """Create dummy game.
        """

        # Create dummy board
        self.board = Board()
        self.board.set_piece(Position(4, 0), Piece(PieceType.King, Colour.WHITE))
        self.board.set_piece(Position(0, 0), Piece(PieceType.Rook, Colour.WHITE))
        self.board.set_piece(Position(0, 1), Piece(PieceType.Pawn, Colour.WHITE))
        self.board.set_piece(Position(7, 0), Piece(PieceType.Rook, Colour.WHITE))
        self.board.set_piece(Position(7, 1), Piece(PieceType.Pawn, Colour.WHITE))
        self.board.set_piece(Position(4, 4), Piece(PieceType.Pawn, Colour.WHITE))

        self.board.set_piece(Position(2, 7), Piece(PieceType.Rook, Colour.BLACK))
        self.board.set_piece(Position(3, 4), Piece(PieceType.Pawn, Colour.BLACK))
        self.board.set_piece(Position(5, 4), Piece(PieceType.Pawn, Colour.BLACK))
        # Create game
        history_moves = [Move(Position(4, 3), Position(4, 4)),
                         Move(Position(3, 6), Position(3, 4))]
        self.game = Game(self.board, Colour.WHITE, history_moves)

    def test_is_piece_touched(self):
        """Test of is_piece_touched() method.
        """

        assert PieceMoves.is_piece_touched(Position(4, 4), self.game)
        assert PieceMoves.is_piece_touched(Position(3, 4), self.game)

    def test_en_passant_moves(self):
        """Test of en_passant_moves() method.
        """

        assert sorted(PieceMoves.en_passant_moves(Position(4, 4), self.game)) == [Move(Position(4, 4), Position(3, 5))]

    def test_castling_moves(self):
        """Test of castling_moves() method.
        """

        assert sorted(PieceMoves.castling_moves(Position(4, 0), self.game)) == [Move(Position(4, 0), Position(6, 0))]

    def test_pawn_moves(self):
        """Test of pawn_moves() method.
        """

        assert sorted(PieceMoves.pawn_moves(Position(4, 4), self.game)) == [Move(Position(4, 4), Position(3, 5)),
                                                                            Move(Position(4, 4), Position(4, 5))]

    def test_king_moves(self):
        """Test of king_moves() method.
        """

        assert sorted(PieceMoves.king_moves(Position(4, 0), self.game)) == [Move(Position(4, 0), Position(3, 0)),
                                                                            Move(Position(4, 0), Position(3, 1)),
                                                                            Move(Position(4, 0), Position(4, 1)),
                                                                            Move(Position(4, 0), Position(5, 0)),
                                                                            Move(Position(4, 0), Position(5, 1)),
                                                                            Move(Position(4, 0), Position(6, 0))]

    def test_all_moves(self):
        """Test of all_moves() method.
        """

        assert sorted(PieceMoves.all_moves(self.game)) == [
                                                           Move(Position(0, 0), Position(1, 0)),
                                                           Move(Position(0, 0), Position(2, 0)),
                                                           Move(Position(0, 0), Position(3, 0)),
                                                           Move(Position(0, 1), Position(0, 2)),
                                                           Move(Position(0, 1), Position(0, 3)),
                                                           Move(Position(4, 0), Position(3, 0)),
                                                           Move(Position(4, 0), Position(3, 1)),
                                                           Move(Position(4, 0), Position(4, 1)),
                                                           Move(Position(4, 0), Position(5, 0)),
                                                           Move(Position(4, 0), Position(5, 1)),
                                                           Move(Position(4, 0), Position(6, 0)),
                                                           Move(Position(4, 4), Position(3, 5)),
                                                           Move(Position(4, 4), Position(4, 5)),
                                                           Move(Position(7, 0), Position(5, 0)),
                                                           Move(Position(7, 0), Position(6, 0)),
                                                           Move(Position(7, 1), Position(7, 2)),
                                                           Move(Position(7, 1), Position(7, 3))]


