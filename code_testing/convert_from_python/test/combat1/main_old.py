import enum
import itertools
from abc import ABC, abstractmethod
import random

random.seed()


class Move(enum.Enum):
    CUT = enum.auto()
    STAB = enum.auto()
    BLOCK = enum.auto()


class AttackType(enum.Enum):
    FAST = enum.auto()
    HEAVY = enum.auto()


# For now, this is a health delta
type MoveResult = int


# Can be used for position and velocity
class VectorState(enum.Enum):
    MID = enum.auto()
    UP = enum.auto()
    DOWN = enum.auto()
    LEFT = enum.auto()
    RIGHT = enum.auto()


def resolve_moves(a: Move, b: Move) -> tuple[MoveResult, MoveResult]:
    print('A {0}s, B {1}s'.format(a.name, b.name))

    lookup: dict[tuple[Move, Move], tuple[MoveResult, MoveResult]] = {
        (Move.CUT, Move.CUT): (-5, -5),
        (Move.CUT, Move.STAB): (-10, -3),
        (Move.CUT, Move.BLOCK): (0, 0),
        (Move.STAB, Move.STAB): (-3, -3),
        (Move.STAB, Move.BLOCK): (0, 0),
        (Move.BLOCK, Move.BLOCK): (0, 0),
    }
    if (a, b) in lookup:
        return lookup[a, b]
    elif (b, a) in lookup:
        return tuple[int, int](reversed(lookup[b, a]))
    else:
        raise "unimpl"

    if a < b:
        return tuple[int, int](reversed(resolve_moves(b, a)))
    if (a, b) == (Move.CUT, Move.CUT):
        return -1, -1
    elif (a, b) == (Move.CUT, Move.STAB):
        return -2, -1
    elif (a, b) == (Move.CUT, Move.BLOCK):
        return 0, 0
    elif (a, b) == (Move.STAB, Move.STAB):
        return -1, -1
    elif (a, b) == (Move.STAB, Move.BLOCK):
        return -1, -1
    elif (a, b) == (Move.BLOCK, Move.BLOCK):
        return -1, -1
    else:
        raise "unimpl"


class Combatant:
    name: str

    health = 30

    # Total pool of energy
    energy = 30

    # How much force one can apply to a blow
    strength: int

    # How quickly one can move
    speed: int

    # How quickly one can begin to react to opponents movements
    reflexes: int

    # # Don't use this one for now.
    # # * Energy rate?
    # # * Energy regen?
    # stamina: int

    def __init__(self, name: str):
        self.name = name

    def is_alive(self) -> bool:
        return self.health > 0

    def get_move(self) -> Move:
        if random.randint(0, 1) < .5:
            return Move.CUT
        return Move.STAB

    def apply_move_result(self, result: MoveResult):
        self.health += result


def battle(a: Combatant, b: Combatant) -> None:
    while a.is_alive() and b.is_alive():
        a_result, b_result = resolve_moves(a.get_move(), b.get_move())
        a.apply_move_result(a_result)
        b.apply_move_result(b_result)
    if not a.is_alive():
        print('A died')
    if not b.is_alive():
        print('B died')


# -----------------------------------------------------------------------------


if __name__ == '__main__':
    battle(Combatant(), Combatant())
