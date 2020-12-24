#!/usr/bin/python3

import unittest

from entities.board import Board


class TestBoard(unittest.TestCase):
	
	def test_create_start_board(self):
		start_board = Board.create_start_board()
		assert start_board is not None

	# def test_failing_assert(self):
	# 	assert 1 == 0
 
