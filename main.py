#!/usr/bin/python3

# pylint: disable=unused-variable

from engine.game import Game
from entities.board import Board


def main():
    start_board = Board.create_start_board()
    game = Game.create_start_game()


if __name__ == "__main__":
    main()
