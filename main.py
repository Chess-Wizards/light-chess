#!/usr/bin/python3

from engine.game import Game
from entities.board import Board
from entities.position import Position


def main():
    start_board = Board.create_start_board()
    game = Game.create_start_game()


if __name__ == "__main__":
    main()
