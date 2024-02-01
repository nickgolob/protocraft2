from combat2.combat2 import *


class AIv1(Combatant):
    def get_move(self, enemy: Combatant) -> Move:
        a, b = self.state, enemy.state
        if self.state == NEUTRAL:
            if enemy.state = NUETRAL:
                return

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
            return ATTACK_PREP_1

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
            return (
                Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, ENERGY_DELTA_ATTACK, 0),
                Reservoirs(HEALTH_DELTA_ATTACK_SUCCESS, ENERGY_DELTA_ATTACK, 0))

        else:
            raise Exception("invalid ", (a.name, b.name))
