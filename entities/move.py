#!/usr/bin/python3

from typing import NamedTuple
from entities.position import Position


class Move(NamedTuple):
    start: Position
    finish: Position