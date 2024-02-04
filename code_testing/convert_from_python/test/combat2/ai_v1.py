from .combat2 import *


class AIv1(Combatant):
    def get_move(self, enemy: Combatant) -> Move:
        return NEUTRAL
        raise "unimplemented"
