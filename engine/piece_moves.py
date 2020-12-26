#!/usr/bin/python3

from __future__ import annotations
from engine.positions_under_threat import PositionsUnderThreat
from entities.position import Position
from entities.pieces import PieceType
from entities.colour import Colour
from typing import List
from engine.game import Game


class PieceMoves(object):
    """Class used to make move. This class is technically PositionsUnderThreat with taking into account
    en_passant and castling.

    ATTENTION: not taking into account check after moves, while doing it for castling!!!
    """

    @staticmethod
    def moves(piece_type: PieceType, position: Position, game: Game) -> List[Position]:
        """Return list of possible moves.
        """

        # Retrieve piece.
        piece = game.board.get_piece(position)
        # Return list of moves using the correct piece method.
        if piece_type.King is piece.type:
            return PieceMoves.king_moves(position, game)
        elif piece_type.Queen is piece.type:
            return PieceMoves.queen_moves(position, game)
        elif piece_type.Bishop is piece.type:
            return PieceMoves.bishop_moves(position, game)
        elif piece_type.Knight is piece.type:
            return PieceMoves.knight_moves(position, game)
        elif piece_type.Rook is piece.type:
            return PieceMoves.rook_moves(position, game)
        elif piece_type.Pawn is piece.type:
            return PieceMoves.pawn_moves(position, game)

    @staticmethod
    def is_piece_touched(pos: Position, game: Game):
        """Check if piece is being touched. Check if there are at least one occurrence of move/touch in history.
        """

        # Iterate over history.
        for move in game.history_moves:
            # Check if pos is being touched at least once.
            if pos == move.start or pos == move.finish:
                return True
        return False

    @staticmethod
    def en_passant_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of en passant moves.
        en passant:
            - opponent pawn makes the move at previous turn
            - opponent pawn jumps over 2 positions
        """

        # Init list of moves.
        en_passant = []
        # Retrieve start piece.
        piece_start = game.board.get_piece(pos)
        # Check if piece_start not empty.
        if piece_start is not None and piece_start.type == PieceType.Pawn:
            # 2 directions.
            shifts = [(-1, 0), (1, 0)]
            for shift_x, shift_y in shifts:
                # Retrieve not start piece.
                piece = game.board.get_piece(Position(pos.x + shift_x, pos.y + shift_y))
                if piece is not None and piece.type == PieceType.Pawn:
                    # Retrieve last_move.
                    last_move = game.history_moves[-1]
                    # Check if pawn makes the last move and if it jumps over 2 positions.
                    if Position(pos.x + shift_x, pos.y + shift_y) == last_move.finish and \
                            abs(last_move.start.y - last_move.finish.y) == 2:
                        # Add move in dependence of colour.
                        if game.turn == Colour.WHITE:
                            en_passant.append(Position(pos.x + shift_x, pos.y + 1))
                        else:
                            en_passant.append(Position(pos.x + shift_x, pos.y - 1))
        return en_passant

    @staticmethod
    def castling_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of castling moves.
           Check is taking into account!!!
        """

        # Init catling list.
        castling = []
        # Retrieve piece at start position.
        piece_start = game.board.get_piece(pos)
        # Retrieve positions under threat (important info for castling).
        pos_under_threat = PositionsUnderThreat.all_positions_under_threat_for_side(game.turn, game.board)
        # Check if piece piece at start position King with no threat/check.
        if piece_start is not None and \
                piece_start.type == PieceType.King and \
                not PieceMoves.is_piece_touched(pos, game) and \
                pos not in pos_under_threat:
            # Short castling. Bunch of conditions.
            if game.board.is_position_empty(Position(pos.x + 1, pos.y)) and \
                    Position(pos.x + 1, pos.y) not in pos_under_threat and \
                    game.board.is_position_empty(Position(pos.x + 2, pos.y)) and \
                    Position(pos.x + 2, pos.y) not in pos_under_threat and \
                    not game.board.is_position_empty(Position(pos.x + 3, pos.y)) and \
                    game.board.get_piece(Position(pos.x + 3, pos.y)).type == PieceType.Rook and \
                    not PieceMoves.is_piece_touched(Position(pos.x + 3, pos.y), game):
                castling.append(Position(pos.x + 2, pos.y))
            # Long castling. Bunch of conditions.
            if game.board.is_position_empty(Position(pos.x - 1, pos.y)) and \
                    Position(pos.x - 1, pos.y) not in pos_under_threat and \
                    game.board.is_position_empty(Position(pos.x - 2, pos.y)) and \
                    Position(pos.x - 2, pos.y) not in pos_under_threat and \
                    game.board.is_position_empty(Position(pos.x - 3, pos.y)) and \
                    not game.board.is_position_empty(Position(pos.x - 4, pos.y)) and \
                    game.board.get_piece(Position(pos.x - 4, pos.y)).type == PieceType.Rook and \
                    not PieceMoves.is_piece_touched(Position(pos.x - 4, pos.y), game):
                castling.append(Position(pos.x - 2, pos.y))
        return castling

    @staticmethod
    def pawn_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of pawn moves.
           pawn moves = positions_under_threat + move forward + en_passant
        """

        # Init move list
        moves = []
        # Check if position under threat occupied with opponent pieces or en_passant possible.
        for pos_under_threat in PositionsUnderThreat.positions_under_pawn_threat(pos, game.turn, game.board):
            if PositionsUnderThreat.is_position_enemy(pos_under_threat, game.turn, game.board) or \
                    pos_under_threat in PieceMoves.en_passant_moves(pos, game):
                moves.append(pos_under_threat)
        # Check forward move.
        shift_forward_y = 1 if game.turn == Colour.WHITE else -1
        pos_forward = Position(pos.x, pos.y + shift_forward_y)
        if game.board.is_position_empty(pos_forward):
            moves.append(pos_forward)
        return moves

    @staticmethod
    def rook_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of rook moves.
           rook moves = positions_under_threat
        """

        return PositionsUnderThreat.positions_under_rook_threat(pos, game.turn, game.board)

    @staticmethod
    def knight_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of knight moves.
           knight moves = positions_under_threat
        """

        return PositionsUnderThreat.positions_under_knight_threat(pos, game.turn, game.board)

    @staticmethod
    def bishop_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of bishop moves.
           bishop moves = positions_under_threat
        """

        return PositionsUnderThreat.positions_under_bishop_threat(pos, game.turn, game.board)

    @staticmethod
    def queen_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of queen moves.
           queen moves = positions_under_threat
        """

        return PositionsUnderThreat.positions_under_queen_threat(pos, game.turn, game.board)

    @staticmethod
    def king_moves(pos: Position, game: Game) -> List[Position]:
        """Return list of king moves.
           king moves = positions_under_threat + castling
        """

        return [*PieceMoves.castling_moves(pos, game),\
                *PositionsUnderThreat.positions_under_king_threat(pos, game.turn, game.board)]
