import random

from combat2.ai_v1 import AIv1
from combat2.combat2 import battle

if __name__ == '__main__':
    random.seed()
    battle(AIv1(name="A"), AIv1(name="B"))
