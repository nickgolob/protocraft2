import random

from agent2 import NeuralNet
from combat import *
from combatant_AI import AI

if __name__ == '__main__':
    random.seed()
    # battle(Combatant(name="A"), Combatant(name="B"))
    nn = NeuralNet()

    for i in range(1000):
        battle(AI("A", nn), AI("B", nn), i)