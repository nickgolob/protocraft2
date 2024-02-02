namespace main;

public class AIv1(string name_) : Combatant(name_) {
    private enum Plan {
      ATTACK, BLOCK      
    }

    private Plan GetPlan(Move my_move, Move opponent_move) {
        switch (opponent_move) {
            case Move.NEUTRAL:
            case Move.ATTACK_PREP_1:
            case Move.ATTACK_HIT:
            case Move.ATTACK_RECOVER:
            case Move.BLOCK:
                return Plan.ATTACK;
            case Move.ATTACK_PREP_2:
                switch (my_move) {
                    case Move.NEUTRAL:
                    case Move.ATTACK_PREP_1:
                    case Move.BLOCK:
                        return Plan.BLOCK;
                    case Move.ATTACK_PREP_2:
                        return Plan.ATTACK;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(my_move), my_move, null);
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(opponent_move), opponent_move, null);
        }
    }
    
    public override Move GetMove(Combatant opponent) {
        if (move == Move.ATTACK_HIT)     return Move.ATTACK_RECOVER;
        if (move == Move.ATTACK_RECOVER) return Move.NEUTRAL;
        
        switch (GetPlan(move, opponent.move)) {
            case Plan.ATTACK:
                switch (move) {
                    case Move.NEUTRAL:
                        return Move.ATTACK_PREP_1;
                    case Move.ATTACK_PREP_1:
                        return Move.ATTACK_PREP_2;
                    case Move.ATTACK_PREP_2:
                        return Move.ATTACK_HIT;
                    case Move.BLOCK:
                        return Move.ATTACK_PREP_1;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            case Plan.BLOCK:
                switch (move) {
                    case Move.NEUTRAL:
                    case Move.ATTACK_PREP_1:
                    case Move.BLOCK:
                        return Move.BLOCK;
                    case Move.ATTACK_PREP_2:
                        return Move.ATTACK_PREP_1;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            default:
                throw new ArgumentOutOfRangeException();    
        }
    }
}