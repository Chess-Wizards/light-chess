#!/usr/bin/python3

import unittest

from entities.board import Board
from entities.pieces import Pieces
from entities.position import Position


class TestBoard(unittest.TestCase):
    def test_set_piece(self):
        board = Board()

        board.set_piece(Position(4, 4), Pieces.WHITE_ROOK)
        # TODO(amirov-m): What is the right order for actual and expected?
        assert board.get_piece(Position(4, 4)) == Pieces.WHITE_ROOK

        board.set_piece(Position(7, 7), Pieces.BLACK_PAWN)
        assert board.get_piece(Position(7, 7)) == Pieces.BLACK_PAWN

    def test_set_piece_rewrite_piece(self):
        board = Board()
        board.set_piece(Position(1, 3), Pieces.WHITE_QUEEN)
        board.set_piece(Position(1, 3), Pieces.BLACK_KING)
        assert board.get_piece(Position(1, 3)) == Pieces.BLACK_KING

    def test_set_piece_invalid_coordinates(self):
        # board = Board()
        # TODO(amirov-m): Try with (-1, 3), (8, 8), (3, 100)
        # InvalidPositionException should be thrown.
        pass

    def test_get_piece(self):
        self.test_set_piece()

    def test_get_piece_invalid_coordinates(self):
        # board = Board()
        # TODO(amirov-m): Try with (-10, 5), (8, 6), (10, 0)

        # InvalidPositionException should be thrown.
        pass

    def test_remove_piece(self):
        board = Board()

        board.set_piece(Position(1, 2), Pieces.BLACK_BISHOP)
        assert board.get_piece(Position(1, 2)) == Pieces.BLACK_BISHOP

        board.remove_piece(Position(1, 2))
        assert board.get_piece(Position(1, 2)) is None

    def test_remove_piece_empty_cell(self):
        board = Board()
        board.remove_piece(Position(3, 4))
        assert board.get_piece(Position(3, 4)) is None

    def test_remove_piece_invalid_coordinates(self):
        # board = Board()
        # TODO(amirov-m): Try with (5, -3), (6, 9), (100, 0)

        # InvalidPositionException should be thrown.
        pass

    def test_get_positions_for_piece(self):
        board = Board()
        board.set_piece(Position(1, 0), Pieces.WHITE_KNIGHT)
        board.set_piece(Position(4, 0), Pieces.WHITE_KNIGHT)
        board.set_piece(Position(0, 7), Pieces.WHITE_KNIGHT)
        board.set_piece(Position(7, 7), Pieces.WHITE_KNIGHT)
        piece_positions = board.get_positions_for_piece(Pieces.WHITE_KNIGHT)
        assert set(piece_positions) == {
            Position(1, 0),
            Position(4, 0),
            Position(0, 7),
            Position(7, 7),
        }

    def test_get_positions_for_piece_after_rewrites(self):
        pass

    def test_get_positions_for_piece_after_remove(self):
        pass

    def test_single_piece_position(self):
        pass

    def test_single_piece_position_raises_expection_many_positions_found(self):
        pass

    def test_create_start_board(self):
        start_board = Board.create_start_board()
        assert start_board is not None
