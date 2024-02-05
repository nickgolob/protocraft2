import random

from combat import *

import numpy
import math
import torch
import collections

# https://pytorch.org/tutorials/intermediate/mario_rl_tutorial.html

# https://github.com/yfeng997/MadMario/blob/master/neural.py
class CustomNet(torch.nn.Module):
    def __init__(self):
        super().__init__()

        raise "unimpl"

        output_dim = len(list(Move))  # possible actions -- every move is the best rated action?
        input_dim = output_dim * 2  # my current state + enemy current state

        self.online = torch.nn.Sequential(
            nn.Conv2d(in_channels=input_dim, out_channels=32, kernel_size=8, stride=4),
            nn.ReLU(),
            nn.Conv2d(in_channels=32, out_channels=64, kernel_size=4, stride=2),
            nn.ReLU(),
            nn.Conv2d(in_channels=64, out_channels=64, kernel_size=3, stride=1),
            nn.ReLU(),
            nn.Flatten(),
            nn.Linear(3136, 512),
            nn.ReLU(),
            nn.Linear(512, output_dim)
        )

        self.target = copy.deepcopy(self.online)
        # Q_target parameters are frozen.
        for p in self.target.parameters():
            p.requires_grad = False

    def forward(self, input, model):
        if model == 'online':
            return self.online(input)
        elif model == 'target':
            return self.target(input)


class Agent:
    def __init__(self):
        self.memory = collections.deque(maxlen=100000)
        self.batch_size = 32

        self.exploration_rate = 1
        self.exploration_rate_decay = 0.99999975
        self.exploration_rate_min = 0.1
        self.gamma = 0.9
        self.curr_step = 0

        self.net = CustomNet()


    def act(self, state) -> Move:
        # EXPLORE
        if numpy.random.rand() < self.exploration_rate:
            action = numpy.random.choice(list(Move))
        # EXPLOIT
        else:
            state = torch.FloatTensor(state)
            state = state.unsqueeze(0)
            action_values = self.net(state, model='online')
            action = torch.argmax(action_values, axis=1).item()
        # decrease exploration_rate
        self.exploration_rate *= self.exploration_rate_decay
        self.exploration_rate = max(self.exploration_rate_min,
                                    self.exploration_rate)
        self.curr_step += 1
        return action

    def cache(self, state, next_state, action, reward: float, done: bool):
        state = torch.FloatTensor(state)
        next_state = torch.FloatTensor(next_state)
        action = torch.LongTensor([action])
        reward = torch.DoubleTensor([reward])
        done = torch.BoolTensor([done])
        self.memory.append((state, next_state, action, reward, done,))

    def recall(self):
        batch = random.sample(self.memory, self.batch_size)
        state, next_state, action, reward, done = map(torch.stack, zip(*batch))
        return state, next_state, action.squeeze(), reward.squeeze(), done.squeeze()

    # --------------------------------------------------------------------------
    # CONFUSION

    def td_estimate(self, state, action):
        current_Q = self.net(state, model='online')[
            numpy.arange(0, self.batch_size), action]  # Q_online(s,a)
        return current_Q

    # @torch.no_grad()
    def td_target(self, reward, next_state, done):
        next_state_Q = self.net(next_state, model='online')
        best_action = torch.argmax(next_state_Q, axis=1)
        next_Q = self.net(next_state, model='target')[
            numpy.arange(0, self.batch_size), best_action]
        return (reward + (1 - done.float()) * self.gamma * next_Q).float()

    def update_Q_online(self, td_estimate, td_target):
        loss = self.loss_fn(td_estimate, td_target)
        self.optimizer.zero_grad()
        loss.backward()
        self.optimizer.step()
        return loss.item()

    def sync_Q_target(self):
        self.net.target.load_state_dict(self.net.online.state_dict())

    def learn(self):
        if self.curr_step % self.sync_every == 0:
            self.sync_Q_target()

        if self.curr_step % self.save_every == 0:
            self.save()

        if self.curr_step < self.burnin:
            return None, None

        if self.curr_step % self.learn_every != 0:
            return None, None

        # Sample from memory
        state, next_state, action, reward, done = self.recall()

        # Get TD Estimate
        td_est = self.td_estimate(state, action)

        # Get TD Target
        td_tgt = self.td_target(reward, next_state, done)

        # Backpropagate loss through Q_online
        loss = self.update_Q_online(td_est, td_tgt)

        return (td_est.mean().item(), loss)

    def save(self):
        save_path = self.save_dir / f"mario_net_{int(self.curr_step // self.save_every)}.chkpt"
        torch.save(
            dict(
                model=self.net.state_dict(),
                exploration_rate=self.exploration_rate
            ),
            save_path
        )
        print(f"MarioNet saved to {save_path} at step {self.curr_step}")

    def load(self, load_path):
        if not load_path.exists():
            raise ValueError(f"{load_path} does not exist")

        ckp = torch.load(load_path,
                         map_location=('cuda' if self.use_cuda else 'cpu'))
        exploration_rate = ckp.get('exploration_rate')
        state_dict = ckp.get('model')

        print(
            f"Loading model at {load_path} with exploration rate {exploration_rate}")
        self.net.load_state_dict(state_dict)
        self.exploration_rate = exploration_rate

    def learn(self):
        raise "unimpl"