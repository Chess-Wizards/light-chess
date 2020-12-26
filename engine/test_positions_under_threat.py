#!/usr/bin/python3

import unittest
from engine.positions_under_threat import PositionsUnderThreat
from entities.board import Board
from entities.position import Position
from entities.pieces import Piece
from entities.pieces import PieceType
from entities.colour import Colour
from engine.game import Game


class TestPositionsUnderThreat(unittest.TestCase):
    """Test of PositionsUnderThreat class.
       Create dummy game by means of setUp() method and check the correctness of pieces moves.

    """

    def setUp(self) -> None:
        """Creates dummy game.
        """

        # Create dummy board
        board = Board()
        board.set_piece(Position(4, 3), Piece(PieceType.Pawn, Colour.WHITE))
        board.set_piece(Position(3, 4), Piece(PieceType.Pawn, Colour.WHITE))
        board.set_piece(Position(5, 4), Piece(PieceType.Pawn, Colour.WHITE))
        board.set_piece(Position(0, 6), Piece(PieceType.Pawn, Colour.WHITE))
        board.set_piece(Position(7, 7), Piece(PieceType.Pawn, Colour.WHITE))
        board.set_piece(Position(4, 4), Piece(PieceType.King, Colour.WHITE))
        board.set_piece(Position(6, 6), Piece(PieceType.Bishop, Colour.WHITE))
        board.set_piece(Position(3, 7), Piece(PieceType.Rook, Colour.WHITE))
        board.set_piece(Position(5, 5), Piece(PieceType.Queen, Colour.WHITE))
        board.set_piece(Position(7, 6), Piece(PieceType.Knight, Colour.WHITE))

        board.set_piece(Position(4, 5), Piece(PieceType.Pawn, Colour.BLACK))
        board.set_piece(Position(5, 7), Piece(PieceType.Bishop, Colour.BLACK))
        # Create dummy game
        self.game = Game(board, Colour.WHITE)

    def test_positions_under_king_threat(self):
        """Test of positions_under_king_threat() method.
        """

        assert sorted(PositionsUnderThreat.positions_under_king_threat(Position(4, 4), self.game)) == [Position(3, 3),
                                                                                                       Position(3, 5),
                                                                                                       Position(4, 5),
                                                                                                       Position(5, 3)]

    def test_positions_under_queen_threat(self):
        """Test of positions_under_queen_threat() method.
        """

        assert sorted(PositionsUnderThreat.positions_under_queen_threat(Position(5, 5), self.game)) == [Position(4, 5),
                                                                                                        Position(4, 6),
                                                                                                        Position(5, 6),
                                                                                                        Position(5, 7),
                                                                                                        Position(6, 4),
                                                                                                        Position(6, 5),
                                                                                                        Position(7, 3),
                                                                                                        Position(7, 5)]

    def test_positions_under_bishop_threat(self):
        """Test of positions_under_bishop_threat() method.
        """

        assert sorted(PositionsUnderThreat.positions_under_bishop_threat(Position(6, 6), self.game)) == [Position(5, 7),
                                                                                                         Position(7, 5)]

    def test_positions_under_knight_threat(self):
        """Test of positions_under_knight_threat() method.
        """

        assert sorted(PositionsUnderThreat.positions_under_knight_threat(Position(7, 6), self.game)) == [Position(5, 7),
                                                                                                         Position(6, 4)]

    def test_positions_under_rook_threat(self):
        """Test of positions_under_rook_threat() method.
        """

        assert sorted(PositionsUnderThreat.positions_under_rook_threat(Position(3, 7), self.game)) == [Position(0, 7),
                                                                                                       Position(1, 7),
                                                                                                       Position(2, 7),
                                                                                                       Position(3, 5),
                                                                                                       Position(3, 6),
                                                                                                       Position(4, 7),
                                                                                                       Position(5, 7)]

    def test_positions_under_pawn_threat(self):
        """Test of positions_under_pawn_threat() method.
        """

        assert not PositionsUnderThreat.positions_under_pawn_threat(Position(4, 3), self.game)
        assert sorted(PositionsUnderThreat.positions_under_pawn_threat(Position(3, 4), self.game)) == [Position(2, 5),
                                                                                                       Position(4, 5)]
        assert sorted(PositionsUnderThreat.positions_under_pawn_threat(Position(5, 4), self.game)) == [Position(4, 5),
                                                                                                       Position(6, 5)]
        assert sorted(PositionsUnderThreat.positions_under_pawn_threat(Position(0, 6), self.game)) == [Position(1, 7)]
        assert not PositionsUnderThreat.positions_under_pawn_threat(Position(7, 7), self.game)
