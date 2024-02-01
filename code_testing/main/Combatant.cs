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
    public static readonly Dictionary<Move, SortedSet<Move>> transitions = new() {
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
}

public class Reservoir(int health_, int energy_, int focus_) {
    public int health { get; } = health_;
    public int energy { get; } = energy_;
    public int focus { get; } = focus_;
}

public class Combatant(string name_) {
    public readonly string name = name_;
    public Reservoir reservoir = new(10, 10, 10);
    public Move move = Move.NEUTRAL;
}