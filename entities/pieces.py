#!/usr/bin/python3

from enum import Enum
from typing import NamedTuple

from entities.colour import Colour


class PieceType(Enum):
	King = 0
	Queen = 1
	Bishop = 2
	Knight = 3
	Rook = 4
	Pawn = 5


class Piece(NamedTuple):
	type: PieceType
	colour: Colour
