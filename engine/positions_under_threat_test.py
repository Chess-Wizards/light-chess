#!/usr/bin/python3

import unittest
from engine.positions_under_threat import PositionsUnderThreat
from entities.board import Board
from entities.position import Position
from entities.pieces import Pieces
from entities.colour import Colour


class TestPositionsUnderThreat(unittest.TestCase):
    """Test of PositionsUnderThreat class.
    Create dummy board by means of setUp() method and check the correctness of pieces moves.
    """

    def setUp(self) -> None:
        """Create dummy game."""

        # Create dummy board
        self.board = Board()
        self.board.set_piece(Position(4, 3), Pieces.WHITE_PAWN)
        self.board.set_piece(Position(3, 4), Pieces.WHITE_PAWN)
        self.board.set_piece(Position(5, 4), Pieces.WHITE_PAWN)
        self.board.set_piece(Position(0, 6), Pieces.WHITE_PAWN)
        self.board.set_piece(Position(7, 7), Pieces.WHITE_PAWN)
        self.board.set_piece(Position(4, 4), Pieces.WHITE_KING)
        self.board.set_piece(Position(6, 6), Pieces.WHITE_BISHOP)
        self.board.set_piece(Position(3, 7), Pieces.WHITE_ROOK)
        self.board.set_piece(Position(5, 5), Pieces.WHITE_QUEEN)
        self.board.set_piece(Position(7, 6), Pieces.WHITE_KNIGHT)

        self.board.set_piece(Position(4, 5), Pieces.BLACK_PAWN)
        self.board.set_piece(Position(5, 7), Pieces.BLACK_ROOK)

    def test_positions_under_king_threat(self):
        """Test of positions_under_king_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_king_threat(
                Position(4, 4), Colour.WHITE, self.board
            )
        ) == [Position(3, 3), Position(3, 5), Position(4, 5), Position(5, 3)]

    def test_positions_under_queen_threat(self):
        """Test of positions_under_queen_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_queen_threat(
                Position(5, 5), Colour.WHITE, self.board
            )
        ) == [
            Position(4, 5),
            Position(4, 6),
            Position(5, 6),
            Position(5, 7),
            Position(6, 4),
            Position(6, 5),
            Position(7, 3),
            Position(7, 5),
        ]

    def test_positions_under_bishop_threat(self):
        """Test of positions_under_bishop_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_bishop_threat(
                Position(6, 6), Colour.WHITE, self.board
            )
        ) == [Position(5, 7), Position(7, 5)]

    def test_positions_under_knight_threat(self):
        """Test of positions_under_knight_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_knight_threat(
                Position(7, 6), Colour.WHITE, self.board
            )
        ) == [Position(5, 7), Position(6, 4)]

    def test_positions_under_rook_threat(self):
        """Test of positions_under_rook_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_rook_threat(
                Position(3, 7), Colour.WHITE, self.board
            )
        ) == [
            Position(0, 7),
            Position(1, 7),
            Position(2, 7),
            Position(3, 5),
            Position(3, 6),
            Position(4, 7),
            Position(5, 7),
        ]

    def test_positions_under_pawn_threat(self):
        """Test of positions_under_pawn_threat() method."""

        assert not PositionsUnderThreat.positions_under_pawn_threat(
            Position(4, 3), Colour.WHITE, self.board
        )
        assert sorted(
            PositionsUnderThreat.positions_under_pawn_threat(
                Position(3, 4), Colour.WHITE, self.board
            )
        ) == [Position(2, 5), Position(4, 5)]
        assert sorted(
            PositionsUnderThreat.positions_under_pawn_threat(
                Position(5, 4), Colour.WHITE, self.board
            )
        ) == [Position(4, 5), Position(6, 5)]
        assert sorted(
            PositionsUnderThreat.positions_under_pawn_threat(
                Position(0, 6), Colour.WHITE, self.board
            )
        ) == [Position(1, 7)]
        assert not PositionsUnderThreat.positions_under_pawn_threat(
            Position(7, 7), Colour.WHITE, self.board
        )
        assert PositionsUnderThreat.positions_under_pawn_threat(
            Position(4, 5), Colour.BLACK, self.board
        ) == [Position(3, 4), Position(5, 4)]

    def test_positions_under_threat(self):
        """Test of positions_under_threat() method."""

        assert sorted(
            PositionsUnderThreat.positions_under_threat(
                Position(4, 4), Pieces.WHITE_KING, self.board
            )
        ) == [Position(3, 3), Position(3, 5), Position(4, 5), Position(5, 3)]

    def test_all_positions_under_threat_for_side(self):
        """Test of all_positions_under_threat_for_side() method."""

        assert sorted(
            PositionsUnderThreat.all_positions_under_threat_for_side(
                Colour.WHITE, self.board
            )
        ) == [
            Position(3, 4),
            Position(3, 7),
            Position(4, 7),
            Position(5, 4),
            Position(5, 5),
            Position(5, 6),
            Position(6, 7),
            Position(7, 7),
        ]
