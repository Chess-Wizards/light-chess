#!/usr/bin/python3

from __future__ import annotations
from engine.game import Game
from entities.move import Move
from entities.pieces import PieceType
from entities.pieces import Piece
from entities.colour import Colour
from entities.board import Board
from entities.position import Position
from engine.positions_under_threat import PositionsUnderThreat
from engine.piece_moves import PieceMoves
import copy


class GameLogic(object):
    """Class used to handle logic. This class is technically utilization of PieceMoves with additional checking of
    check after each move.
    """

    @staticmethod
    def is_check(board: Board, colour: Colour) -> bool:
        """ Check if check occurs.
        """

        # Retrieve king position
        king_pos = board.get_positions_for_piece(Piece(PieceType.King, colour))[0]
        return king_pos in PositionsUnderThreat.all_positions_under_threat_for_side(colour, board)

    @staticmethod
    def make_move(move: Move, game: Game) -> Game:
        """ Make move.
        """

        # Copy game (pass by value)
        game = copy.deepcopy(game)
        # Retrieve piece at start position
        piece = game.board.get_piece(move.start)
        # Get possible moves
        possible_moves = PieceMoves.moves(piece.type, move.start, game)
        # Check if move satisfies
        if move.finish in possible_moves:
            # Check if castling occurs
            if move.finish in PieceMoves.castling_moves(move.start, game):
                game.board.set_piece(Position(int((move.finish.x+move.start.x)/2), move.start.y),
                                     Piece(PieceType.Rook, game.turn))
                # Short castling
                if move.finish.x-move.start.x > 0:
                    game.board.remove_piece(Position(7, move.start.y))
                # Long castling
                else:
                    game.board.remove_piece(Position(0, move.start.y))
            # Check if en passant occurs
            if move.finish in PieceMoves.en_passant_moves(move.start, game):
                game.board.remove_piece(Position(move.finish.x, move.start.y))
            # Update board
            game.board.set_piece(move.finish, piece)
            game.board.remove_piece(move.start)
            # Update history
            game.history_moves.append(move)
        return Game(game.board, Colour.change_colour(game.turn), game.history_moves)

    @staticmethod
    def is_move_possible(game: Game, move: Move) -> bool:
        """ Check if move possible.
        """

        # Get piece at start position
        piece = game.board.get_piece(move.start)
        # Check if figure exists.
        if piece is None:
            return False
        # Check if colours match.
        if game.turn != piece.colour:
            return False
        # Make move
        next_game = GameLogic.make_move(move, game)
        # Check if check occurs after making move
        if GameLogic.is_check(next_game.board, next_game.turn):
            return True
        else:
            return False