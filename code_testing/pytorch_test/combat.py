import enum

# Structs: https://stackoverflow.com/questions/35988/c-like-structures-in-python
from dataclasses import dataclass


# ------------------------------------------------------------------------------
# ------------------------------------------------------------------------------

@dataclass
class Reservoirs:
    # zero = death
    health: int
    # Total pool of physical energy. Used for attacking.
    energy: int
    # Total pool of mental energy. Used for blocking
    focus: int

    def __iadd__(self, other: 'Reservoirs'):
        self.health += other.health
        self.energy += other.energy
        self.focus += other.focus
        return self


# Not used yet
@dataclass
class Attributes:
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


# Per "frame"
HEALTH_RECHARGE_RATE = 0
ENERGY_RECHARGE_RATE = 1
FOCUS_RECHARGE_RATE = 1

HEALTH_DELTA_ATTACK_SUCCESS = -1
ENERGY_DELTA_ATTACK = -1
FOCUS_DELTA_BLOCK = 2


# ------------------------------------------------------------------------------
# ------------------------------------------------------------------------------
# Combat state diagram:

class Move(enum.Enum):
    NEUTRAL = enum.auto()

    # windup - 1
    ATTACK_PREP_1 = enum.auto()
    # windup - 2
    ATTACK_PREP_2 = enum.auto()
    # swing + hit
    ATTACK_HIT = enum.auto()
    ATTACK_RECOVER = enum.auto()
    BLOCK = enum.auto()


NEUTRAL = Move.NEUTRAL
ATTACK_PREP_1 = Move.ATTACK_PREP_1
ATTACK_PREP_2 = Move.ATTACK_PREP_2
ATTACK_HIT = Move.ATTACK_HIT
ATTACK_RECOVER = Move.ATTACK_RECOVER
BLOCK = Move.BLOCK

states: dict[Move, list[Move]] = {
    NEUTRAL: [
        NEUTRAL,
        ATTACK_PREP_1,
        BLOCK,
    ],
    ATTACK_PREP_1: [
        NEUTRAL,
        ATTACK_PREP_2,
        BLOCK,
    ],
    ATTACK_PREP_2: [
        ATTACK_PREP_1,
        ATTACK_HIT,
    ],
    ATTACK_HIT: [
        ATTACK_RECOVER,
    ],
    ATTACK_RECOVER: [
        NEUTRAL,
    ],
    BLOCK: [
        NEUTRAL,
        ATTACK_PREP_1,
        BLOCK,
    ]
}


def validate_states():
    for state, edges in states.items():
        last_index = -1
        for edge in edges:
            this_index = list(states).index(edge)
            assert this_index > last_index
            last_index = this_index


validate_states()

def is_valid_move(start: Move, end: Move) -> bool:
    return end in states[start]

# ------------------------------------------------------------------------------
# ------------------------------------------------------------------------------


class Combatant:
    name: str
    reservoirs = Reservoirs(health=10, energy=10, focus=10)
    move: Move = Move.NEUTRAL

    def __init__(self, name: str):
        self.name = name

    def is_alive(self) -> bool:
        return self.reservoirs.health > 0

    def get_move(self, enemy: 'Combatant') -> Move:
        raise "virtual"

    def invalid_move(self, move: Move) -> None:
        pass

    def apply_move_result(self, deltas: Reservoirs) -> None:
        self.reservoirs += deltas

    def post_move_update(self, enemy: 'Combatant') -> None:
        pass


def resolve_moves(a: Move, b: Move) -> tuple[Reservoirs, Reservoirs]:
    if list(Move).index(b) < list(Move).index(a):
        return tuple[Reservoirs, Reservoirs](reversed(resolve_moves(b, a)))

    # Can't use a case: https://stackoverflow.com/questions/66159432/how-to-use-values-stored-in-variables-as-case-patterns
    # All non attacks:
    if (
            (a, b) == (NEUTRAL, NEUTRAL) or
            (a, b) == (NEUTRAL, ATTACK_PREP_1) or
            (a, b) == (NEUTRAL, ATTACK_PREP_2) or
            (a, b) == (NEUTRAL, ATTACK_RECOVER) or
            (a, b) == (NEUTRAL, BLOCK) or
            (a, b) == (ATTACK_PREP_1, ATTACK_PREP_1) or
            (a, b) == (ATTACK_PREP_1, ATTACK_PREP_2) or
            (a, b) == (ATTACK_PREP_1, ATTACK_RECOVER) or
            (a, b) == (ATTACK_PREP_1, BLOCK) or
            (a, b) == (ATTACK_PREP_2, ATTACK_PREP_2) or
            (a, b) == (ATTACK_PREP_2, ATTACK_RECOVER) or
            (a, b) == (ATTACK_PREP_2, BLOCK) or
            (a, b) == (ATTACK_RECOVER, ATTACK_RECOVER) or
            (a, b) == (ATTACK_RECOVER, BLOCK) or
            (a, b) == (BLOCK, BLOCK) or
            False):
        return Reservoirs(0, 0, 0), Reservoirs(0, 0, 0)

    # Attacks:
    elif (a, b) == (ATTACK_HIT, ATTACK_RECOVER):
        return (Reservoirs(0, ENERGY_DELTA_ATTACK, 0),
                Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, 0, 0))

    elif (a, b) == (ATTACK_HIT, BLOCK):
        return (Reservoirs(0, ENERGY_DELTA_ATTACK, 0),
                Reservoirs(0, 0, FOCUS_DELTA_BLOCK))

    elif (
            (a, b) == (NEUTRAL, ATTACK_HIT) or
            (a, b) == (ATTACK_PREP_1, ATTACK_HIT) or
            (a, b) == (ATTACK_PREP_2, ATTACK_HIT) or
            False):
        return (Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, 0, 0),
                Reservoirs(0, ENERGY_DELTA_ATTACK, 0))

    elif (a, b) == (ATTACK_HIT, ATTACK_HIT):
        return (Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, ENERGY_DELTA_ATTACK, 0),
                Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, ENERGY_DELTA_ATTACK, 0))

    else:
        raise Exception("invalid ", (a.name, b.name))


def battle(a: Combatant, b: Combatant, game_count: int) -> None:
    print_moves = game_count % 50 == 0
    timer = 200
    invalid_move_played = False
    while a.is_alive() and b.is_alive() and timer > 0:
        a_move, b_move = a.get_move(b), b.get_move(a)
        if print_moves:
            print("a,b: ", a_move, b_move)

        if not is_valid_move(a.move, a_move) or not is_valid_move(b.move, b_move):
            if not is_valid_move(a.move, a_move):
                a.invalid_move(a_move)
            if not is_valid_move(b.move, b_move):
                b.invalid_move(b_move)
            invalid_move_played = True
            break

        a_result, b_result = resolve_moves(a_move, b_move)
        a.state = a_result
        b.state = b_result
        a.apply_move_result(a_result)
        b.apply_move_result(b_result)
        a.post_move_update(b)
        b.post_move_update(a)
        timer -= 1
    if invalid_move_played:
        print(game_count, "- invalid move played")
    elif not a.is_alive() and not b.is_alive():
        print(game_count, "- both died")
    elif not a.is_alive():
        print(game_count, '- A died')
    elif not b.is_alive():
        print(game_count, '- B died')
    if timer == 0:
        print(game_count, '- TIMEOUT')
