from combat import Move


# def get_state(my_location: Move):
#     state = tuple(1 if my_location == itr else 0 for itr in Move) + tuple(1 if my_location == itr else 0 for itr in Move)
#     print(state)
# get_state(Move.ATTACK_HIT)

print(list(Move).index(Move.ATTACK_PREP_2))