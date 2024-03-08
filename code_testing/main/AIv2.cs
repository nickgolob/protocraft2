namespace main;

// General strategy: block when possible. 
public class AIv2(string name_) : Combatant(name_) {
  private enum Plan {
    ZONING_ATTACK,
    SHORT_ATTACK,
    BLOCK
  }

  private Plan GetPlan(Combatant opponent) {
    return Plan.BLOCK;
  }

  public override Move GetMove(Combatant opponent) {
    return Move.BLOCK;
  }
}