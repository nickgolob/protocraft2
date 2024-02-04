import random

from combat import *

if __name__ == '__main__':
    random.seed()
    battle(Combatant(name="A"), Combatant(name="B"))
