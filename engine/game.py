#!/usr/bin/python3

from __future__ import annotations

from typing import NamedTuple, List

from entities.board import Board
from entities.colour import Colour
from entities.move import Move


class Game(NamedTuple):
    board: Board
    turn: Colour
    history_moves: List[Move] = []

    @staticmethod
    def create_start_game() -> Game:
        start_board = Board.create_start_board()
        return Game(start_board, Colour.WHITE)
