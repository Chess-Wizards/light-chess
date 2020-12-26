#!/usr/bin/python3

from __future__ import annotations

from entities.position import Position
from entities.board import Board
from engine.game import Game
from typing import List
import itertools


class PositionsUnderThreat(object):
    """Class returns list of positions under thread. 'under thread' means all positions which enemy king
    cannot reach, because of the check.

    """

    @staticmethod
    def _is_position_enemy(pos: Position, game: Game) -> bool:
        """Check if position occupied with enemy piece.
        """

        # Retrieve piece.
        piece = game.board.get_piece(pos)
        return piece is not None and piece.colour != game.turn

    @staticmethod
    def _is_position_enemy_or_empty(pos: Position, game: Game) -> bool:
        """Check if position occupied with enemy piece or empty.
           Return False if pos is out of the board.
        """

        # Check if pos locates on the board.
        if Board.is_position_on_board(pos, game.board):
            return PositionsUnderThreat._is_position_enemy(pos, game) or game.board.is_position_empty(pos)
        else:
            return False

    @staticmethod
    def check_positions(pos: Position, game: Game, shifts: list) -> list:
        """Take input list of shifts and verify which of them are possible.
        """

        # Init position list.
        positions_under_threat = []
        # Iterate through shifts.
        for shift_x, shift_y in shifts:
            # Check if shift is possible, otherwise break the loop.
            if PositionsUnderThreat._is_position_enemy_or_empty(Position(pos.x+shift_x,
                                                                         pos.y+shift_y), game):
                # Add shift
                positions_under_threat.append(Position(pos.x+shift_x,
                                                       pos.y+shift_y))
        return positions_under_threat

    @staticmethod
    def check_positions_with_obstacles(pos: Position, game: Game, shifts: list) -> list:
        """Take input list of shifts and verify which of them are possible.
           Iterate through shifts till the first obstacle. Obstacle means own or enemy piece.
        """

        # Init position list.
        positions_under_threat = []
        # Iterates though shifts.
        for shift_x, shift_y in shifts:
            # Check if shift is possible, otherwise break the loop.
            if not PositionsUnderThreat._is_position_enemy_or_empty(Position(pos.x+shift_x, pos.y+shift_y), game):
                break
            # Add shift
            positions_under_threat.append(Position(pos.x+shift_x,
                                                   pos.y+shift_y))
            # Check if position is occupied with enemy piece, otherwise break the loop.
            if PositionsUnderThreat._is_position_enemy(Position(pos.x+shift_x, pos.y+shift_y), game):
                break
        return positions_under_threat

    @staticmethod
    def positions_under_king_threat(pos: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by king.
        """

        # Init list of shifts
        shifts = list(itertools.product([-1, 0, 1], repeat=2))
        return PositionsUnderThreat.check_positions(pos, game, shifts)

    @staticmethod
    def positions_under_queen_threat(position: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by queen.
           Can be calculated by combining returns from bishop and rook methods.
        """

        return [*PositionsUnderThreat.positions_under_rook_threat(position, game),
                                  *PositionsUnderThreat.positions_under_bishop_threat(position, game)]

    @staticmethod
    def positions_under_bishop_threat(pos: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by bishop.
           Check 4 directions till the first obstacle: up_right, down_right, down_left and up_left.
        """

        # Init lists of shifts.
        shifts_up_right = [(shift, shift) for shift in range(1, game.board.height)]
        shifts_down_right = [(shift, -shift) for shift in range(1, game.board.height)]
        shifts_down_left = [(-shift, -shift) for shift in range(1, game.board.height)]
        shifts_up_left = [(-shift, shift) for shift in range(1, game.board.height)]
        # Init position list.
        positions_under_threat = []
        # Iterate over 4 directions.
        for shifts in [shifts_up_right, shifts_down_right, shifts_down_left, shifts_up_left]:
            positions_under_threat = [*positions_under_threat,
                                      *PositionsUnderThreat.check_positions_with_obstacles(pos, game, shifts)]
        return positions_under_threat

    @staticmethod
    def positions_under_knight_threat(pos: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by knight.
        """

        # Init list of shifts.
        shifts = [*list(itertools.product([-1, 1], [-2, 2])), *list(itertools.product([-2, 2], [-1, 1]))]
        return PositionsUnderThreat.check_positions(pos, game, shifts)

    @staticmethod
    def positions_under_rook_threat(pos: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by rook.
           Check 4 directions till the first obstacle: up, right, down and left.
        """

        # Init lists of shifts.
        shifts_up = [(0, shift_y) for shift_y in range(1, game.board.height)]
        shifts_right = [(shift_x, 0) for shift_x in range(1, game.board.width)]
        shifts_down = [(0, shift_y) for shift_y in range(-1, -game.board.height, -1)]
        shifts_left = [(shift_x, 0) for shift_x in range(-1, -game.board.width, -1)]
        # Init position list.
        positions_under_threat = []
        # Iterate over directions
        for shifts in [shifts_up, shifts_right, shifts_down, shifts_left]:
            positions_under_threat = [*positions_under_threat,
                                      *PositionsUnderThreat.check_positions_with_obstacles(pos, game, shifts)]
        return positions_under_threat

    @staticmethod
    def positions_under_pawn_threat(pos: Position, game: Game) -> List[Position]:
        """Return list of positions under threat by pawn.
        """

        # Init list of shifts.
        shifts = [(-1, 1), (1, 1)]
        return PositionsUnderThreat.check_positions(pos, game, shifts)
    

