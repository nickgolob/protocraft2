from agent2 import NeuralNet
from combat import *

import torch

import numpy


def _get_state(my_location: Move, enemy_location: Move):
    state = (tuple(1 if my_location == itr else 0 for itr in Move) +
             tuple(1 if enemy_location == itr else 0 for itr in Move))
    return numpy.array(state, dtype=int)

def _act_to_move(nn_action) -> Move:
    # print(nn_act)
    # print(max(nn_act))
    return list(Move)[max(nn_action)]

def _move_to_act(move: Move):
    return tuple(1 if move == itr else 0 for itr in Move)

class AI(Combatant):
    def __init__(self, name: str, neural_net: NeuralNet):
        super().__init__(name)
        self.neural_net = neural_net

    def _get_reward_done(self, enemy: Combatant) -> tuple[int, bool]:
        took_damage = enemy.move == Move.ATTACK_HIT
        did_damage = self.move == Move.ATTACK_HIT
        reward = -1 if took_damage else 0 + 1 if did_damage else 0
        done = self.reservoirs.health <= 0 or enemy.reservoirs.health <= 0
        return reward, done

    def get_move(self, enemy: Combatant) -> Move:
        self.curr_state = _get_state(self.move, enemy.move)
        nn_act = self.neural_net.get_act(_get_state(self.move, enemy.move))
        return _act_to_move(nn_act)

    def post_move_update(self, enemy: Combatant) -> None:
        old_state = self.curr_state
        new_state = _get_state(self.move, enemy.move)
        reward, done = self._get_reward_done(enemy)
        self.neural_net.update(old_state, _move_to_act(self.move), new_state, reward, done)
