#!/usr/bin/python3

from enum import Enum
from typing import NamedTuple

from entities.colour import Colour


class PieceType(Enum):
    KING = 0
    QUEEN = 1
    BISHOP = 2
    KNIGHT = 3
    ROOK = 4
    PAWN = 5


class Piece(NamedTuple):
    type: PieceType
    colour: Colour


class Pieces(NamedTuple):
    WHITE_KING = Piece(PieceType.KING, Colour.WHITE)
    WHITE_QUEEN = Piece(PieceType.QUEEN, Colour.WHITE)
    WHITE_BISHOP = Piece(PieceType.BISHOP, Colour.WHITE)
    WHITE_KNIGHT = Piece(PieceType.KNIGHT, Colour.WHITE)
    WHITE_ROOK = Piece(PieceType.ROOK, Colour.WHITE)
    WHITE_PAWN = Piece(PieceType.PAWN, Colour.WHITE)

    BLACK_KING = Piece(PieceType.KING, Colour.BLACK)
    BLACK_QUEEN = Piece(PieceType.QUEEN, Colour.BLACK)
    BLACK_BISHOP = Piece(PieceType.BISHOP, Colour.BLACK)
    BLACK_KNIGHT = Piece(PieceType.KNIGHT, Colour.BLACK)
    BLACK_ROOK = Piece(PieceType.ROOK, Colour.BLACK)
    BLACK_PAWN = Piece(PieceType.PAWN, Colour.BLACK)
