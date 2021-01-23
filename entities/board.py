#!/usr/bin/python3

from __future__ import annotations

from collections import defaultdict

from entities.colour import Colour
from entities.pieces import Piece, Pieces, PieceType
from entities.position import Position
from typing import List, Optional


class SinglePositionNotFoundException(Exception):
    def __init__(self, actual_positions_count: int) -> None:
        self._actual_positions_count = actual_positions_count

    def actual_positions_count(self) -> int:
        return self._actual_positions_count


class Board(object):
    """
    Represents a chess board.
    """

    def __init__(self) -> None:
        # Stores mapping from unique pairs of (piece_type, colour)
        # to a set of positions.
        # Used for getting all positions for a specific piece type.
        self._piece_to_pos = defaultdict(set)
        # Stores mapping from position to a piece description.
        # Used for getting a piece standing on a position.
        self._pos_to_piece = dict()

    # Set a provided piece on a specified position.
    def set_piece(self, pos: Position, piece: Piece) -> None:
        self.remove_piece(pos)

        self._piece_to_pos[piece].add(pos)
        self._pos_to_piece[pos] = piece

    # Return a piece on a specified position.
    #
    # Method returns None if there is no piece on a specified position.
    def get_piece(self, pos) -> Optional[Piece]:
        return self._pos_to_piece.get(pos)

    # Remove piece from a specified position.
    #
    # If there is no piece on a specified position, do nothig.
    def remove_piece(self, pos) -> None:
        piece_to_remove = self._pos_to_piece.get(pos)
        if piece_to_remove is not None:
            self._piece_to_pos[piece_to_remove].remove(pos)
            del self._pos_to_piece[pos]

    # Return positions of a specific piece.
    def get_positions_for_piece(self, piece: Piece) -> List[Position]:
        return list(self._piece_to_pos[piece])

    # Return the position of a specific piece.
    #
    # If several or 0 positions were found, throw SinglePositionNotFoundError.
    def get_single_piece_position(self, piece) -> Position:
        piece_positions = self.get_positions_for_piece(piece)
        if len(piece_positions) != 1:
            raise SinglePositionNotFoundException(len(piece_positions))
        return piece_positions[0]

    # Create a chess board with a default start position.
    @staticmethod
    def create_start_board() -> Board:
        board = Board()

        # Set white pieces.
        board.set_piece(Position(0, 0), Pieces.WHITE_ROOK)
        board.set_piece(Position(1, 0), Pieces.WHITE_KNIGHT)
        board.set_piece(Position(2, 0), Pieces.WHITE_BISHOP)
        board.set_piece(Position(3, 0), Pieces.WHITE_QUEEN)
        board.set_piece(Position(4, 0), Pieces.WHITE_KING)
        board.set_piece(Position(5, 0), Pieces.WHITE_BISHOP)
        board.set_piece(Position(6, 0), Pieces.WHITE_KNIGHT)
        board.set_piece(Position(7, 0), Pieces.WHITE_ROOK)

        board.set_piece(Position(0, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(1, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(2, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(3, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(4, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(5, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(6, 1), Pieces.WHITE_PAWN)
        board.set_piece(Position(7, 1), Pieces.WHITE_PAWN)

        # Set black pieces.
        board.set_piece(Position(0, 7), Pieces.BLACK_ROOK)
        board.set_piece(Position(1, 7), Pieces.BLACK_KNIGHT)
        board.set_piece(Position(2, 7), Pieces.BLACK_BISHOP)
        board.set_piece(Position(3, 7), Pieces.BLACK_QUEEN)
        board.set_piece(Position(4, 7), Pieces.BLACK_KING)
        board.set_piece(Position(5, 7), Pieces.BLACK_BISHOP)
        board.set_piece(Position(6, 7), Pieces.BLACK_KNIGHT)
        board.set_piece(Position(7, 7), Pieces.BLACK_ROOK)

        board.set_piece(Position(0, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(1, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(2, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(3, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(4, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(5, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(6, 6), Pieces.BLACK_PAWN)
        board.set_piece(Position(7, 6), Pieces.BLACK_PAWN)

        return board
