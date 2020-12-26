#!/usr/bin/python3

from __future__ import annotations

from typing import NamedTuple
from entities.board import Board
from entities.colour import Colour
from entities.position import Position


class Game(NamedTuple):
	board: Board
	turn: Colour

	@staticmethod
	def create_start_game() -> Game:
		start_board = Board.create_start_board()
		return Game(start_board, Colour.WHITE)

	@staticmethod
	def make_move(start_pos: Position, finish_pos: Position) -> Game:
		piece = Game.board.get_piece(start_pos)
		Game.board.set_piece(finish_pos, piece)
		Game.board.remove_piece(start_pos)
        
