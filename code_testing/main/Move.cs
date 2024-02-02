namespace main;

public enum Move {
    NEUTRAL,
    ATTACK_PREP_1,
    ATTACK_PREP_2,
    ATTACK_HIT,
    ATTACK_RECOVER,
    BLOCK,
}

public static class Transitions {
    public static readonly SortedDictionary<Move, SortedSet<Move>> transitions =
        new SortedDictionary<Move, SortedSet<Move>>() {
            {
                Move.NEUTRAL,
                [Move.NEUTRAL, Move.ATTACK_PREP_1, Move.BLOCK,]
            }, {
                Move.ATTACK_PREP_1,
                [Move.NEUTRAL, Move.ATTACK_PREP_2, Move.BLOCK,]
            }, {
                Move.ATTACK_PREP_2,
                [Move.NEUTRAL, Move.ATTACK_PREP_1,]
            }, {
                Move.ATTACK_HIT,
                [Move.ATTACK_RECOVER,]
            }, {
                Move.ATTACK_RECOVER,
                [Move.NEUTRAL,]
            }, {
                Move.BLOCK,
                [Move.NEUTRAL, Move.ATTACK_PREP_1, Move.BLOCK]
            },
        };

    public static bool IsValid(Move from, Move to) {
        return transitions[from].Contains(to);
    }
}