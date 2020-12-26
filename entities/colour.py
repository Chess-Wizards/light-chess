#!/usr/bin/python3

from __future__ import annotations

from enum import Enum


class Colour(Enum):
	WHITE = 0
	BLACK = 1

	@staticmethod
	def change_colour(colour: Colour):
		if colour == colour.WHITE:
			return colour.BLACK
		else:
			return colour.WHITE

