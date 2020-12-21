#!/usr/bin/python3

from __future__ import annotations

from .pieces import Piece, PieceType, PieceColour
from .position import Position
from typing import Optional

class Board(object):
	"""
	Represents a chess board.
	"""

	def __init__(self) -> None:
		self._board = [[None] * 8] * 8

	# Set a provided piece on a specified position.
	def set_piece(self, pos: Position, piece: Piece) -> None:
		self._board[pos[0]][pos[1]] = piece

	# Return a piece on a specified position.
	# 
	# Method returns None if there is no piece on a specified position.
	def get_piece(self, pos) -> Optional[Piece]:
		return self._board[pos[0]][pos[1]]

	# Create a chess board with a default start position.
	@staticmethod
	def create_start_board() -> Board:
		board = Board()

		# Set white pieces.
		board.set_piece(Position(0, 0), Piece(PieceType.Rook, PieceColour.White))
		board.set_piece(Position(1, 0), Piece(PieceType.Knight, PieceColour.White))
		board.set_piece(Position(2, 0), Piece(PieceType.Bishop, PieceColour.White))
		board.set_piece(Position(3, 0), Piece(PieceType.Queen, PieceColour.White))
		board.set_piece(Position(4, 0), Piece(PieceType.King, PieceColour.White))
		board.set_piece(Position(5, 0), Piece(PieceType.Bishop, PieceColour.White))
		board.set_piece(Position(6, 0), Piece(PieceType.Knight, PieceColour.White))
		board.set_piece(Position(7, 0), Piece(PieceType.Rook, PieceColour.White))

		# TODO(amirovm): Add white pawns.

		# Set black pieces.

		# TODO(amirovm): Add black pieces.

		return board