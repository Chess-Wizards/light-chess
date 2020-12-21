#!/usr/bin/python3

from __future__ import annotations

from typing import NamedTuple
from entities.board import Board
from entities.colour import Colour

class Game(NamedTuple):
	board: Board
	turn: Colour

	@staticmethod
	def create_start_game() -> Game:
		start_board = Board.create_start_board()
		return Game(start_board, Colour.WHITE)
