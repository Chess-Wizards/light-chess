#!/usr/bin/python3

from __future__ import annotations

from enum import Enum


class Colour(Enum):
    WHITE = 0
    BLACK = 1

    def invert(self) -> Colour:
        if self == Colour.WHITE:
            return Colour.BLACK
        return Colour.WHITE
