#!/usr/bin/python3

from entities.board import Board
from entities.position import Position

def main():
	start_board = Board.create_start_board()

	for i in range(8):
		for j in range(8):
			print('Position %d %d: %s' % (i, j, start_board.get_piece(Position(i, j))))

if __name__ == '__main__':
	main()
