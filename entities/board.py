#!/usr/bin/python3

from __future__ import annotations

from collections import defaultdict

from entities.colour import Colour
from entities.pieces import Piece, PieceType
from entities.position import Position
from typing import List, Optional


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
		# Stores board characteristic
		self.min_y, self.max_y = 0, 7
		self.min_x, self.max_x = 0, 7
		self.height = self.max_y - self.min_y + 1
		self.width = self.max_x - self.min_x + 1

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

	def is_position_empty(self, pos):
		return self.get_piece(pos) is None

	# Remove piece from a specified position.
	#
	# If there is no piece on a specified position, do nothig.
	def remove_piece(self, pos) -> None:
		piece_to_remove = self._pos_to_piece.get(pos)
		if piece_to_remove is not None:
			self._piece_to_pos[piece_to_remove].remove(pos)
			del self._pos_to_piece[pos]

	# Return the position of a specific piece.
	#
	# If several or 0 positions were found, throw SinglePositionNotFoundError.
	def get_single_piece_position(self, piece) -> Position:
		pass

	# Return positions of a specific piece.
	def get_positions_for_piece(self, piece: Piece) -> List[Position]:
		return list(self._piece_to_pos[piece])

	# Return all positions for one side
	def get_positions_for_side(self, colour: Colour):
		side_positions = []
		for _, positions in self._piece_to_pos.items():
			side_positions.extend([pos for pos in positions
							 	   if self.get_piece(pos) is not None and self.get_piece(pos).colour == colour])
		return side_positions

	# Check if position locates inside board
	@staticmethod
	def is_position_on_board(pos: Position, board: Board) -> bool:
		return board.min_x <= pos.x <= board.max_x and board.min_y <= pos.y <= board.max_y

	# Create a chess board with a default start position.
	@staticmethod
	def create_start_board() -> Board:
		board = Board()

		# Set white pieces.
		board.set_piece(Position(0, 0), Piece(PieceType.Rook, Colour.WHITE))
		board.set_piece(Position(1, 0), Piece(PieceType.Knight, Colour.WHITE))
		board.set_piece(Position(2, 0), Piece(PieceType.Bishop, Colour.WHITE))
		board.set_piece(Position(3, 0), Piece(PieceType.Queen, Colour.WHITE))
		board.set_piece(Position(4, 0), Piece(PieceType.King, Colour.WHITE))
		board.set_piece(Position(5, 0), Piece(PieceType.Bishop, Colour.WHITE))
		board.set_piece(Position(6, 0), Piece(PieceType.Knight, Colour.WHITE))
		board.set_piece(Position(7, 0), Piece(PieceType.Rook, Colour.WHITE))

		board.set_piece(Position(0, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(1, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(2, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(3, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(4, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(5, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(6, 1), Piece(PieceType.Pawn, Colour.WHITE))
		board.set_piece(Position(7, 1), Piece(PieceType.Pawn, Colour.WHITE))

		# Set black pieces.
		board.set_piece(Position(0, 7), Piece(PieceType.Rook, Colour.BLACK))
		board.set_piece(Position(1, 7), Piece(PieceType.Knight, Colour.BLACK))
		board.set_piece(Position(2, 7), Piece(PieceType.Bishop, Colour.BLACK))
		board.set_piece(Position(3, 7), Piece(PieceType.Queen, Colour.BLACK))
		board.set_piece(Position(4, 7), Piece(PieceType.King, Colour.BLACK))
		board.set_piece(Position(5, 7), Piece(PieceType.Bishop, Colour.BLACK))
		board.set_piece(Position(6, 7), Piece(PieceType.Knight, Colour.BLACK))
		board.set_piece(Position(7, 7), Piece(PieceType.Rook, Colour.BLACK))

		board.set_piece(Position(0, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(1, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(2, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(3, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(4, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(5, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(6, 6), Piece(PieceType.Pawn, Colour.BLACK))
		board.set_piece(Position(7, 6), Piece(PieceType.Pawn, Colour.BLACK))

		return board
