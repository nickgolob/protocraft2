namespace main;

public static class Battle {
  private static (Reservoir.Delta, Reservoir.Delta)
      ResolveReservoirDeltas(Move a, Move b) {
    if (b < a) {
      var (b_result, a_result) = ResolveReservoirDeltas(b, a);
      return (a_result, b_result);
    }
    if (b < a) { throw new Exception(); }

    const int kHealthDeltaAttackSuccess = -1;
    const int kEnergyDeltaAttack = -1;
    const int kFocusDeltaBlock = 2;

    switch (a, b) {
      case (Move.NEUTRAL, Move.NEUTRAL):
      case (Move.NEUTRAL, Move.ATTACK_PREP_1):
      case (Move.NEUTRAL, Move.ATTACK_PREP_2):
      case (Move.NEUTRAL, Move.ATTACK_RECOVER):
      case (Move.NEUTRAL, Move.BLOCK):
      case (Move.ATTACK_PREP_1, Move.ATTACK_PREP_1):
      case (Move.ATTACK_PREP_1, Move.ATTACK_PREP_2):
      case (Move.ATTACK_PREP_1, Move.ATTACK_RECOVER):
      case (Move.ATTACK_PREP_1, Move.BLOCK):
      case (Move.ATTACK_PREP_2, Move.ATTACK_PREP_2):
      case (Move.ATTACK_PREP_2, Move.ATTACK_RECOVER):
      case (Move.ATTACK_PREP_2, Move.BLOCK):
      case (Move.ATTACK_RECOVER, Move.ATTACK_RECOVER):
      case (Move.ATTACK_RECOVER, Move.BLOCK):
      case (Move.BLOCK, Move.BLOCK):
        return (new Reservoir.Delta(0, 0, 0), new Reservoir.Delta(0, 0, 0));
      case (Move.ATTACK_HIT, Move.ATTACK_RECOVER):
        return (new Reservoir.Delta(0, kEnergyDeltaAttack, 0),
            new Reservoir.Delta(kHealthDeltaAttackSuccess, 0, 0));
      case (Move.ATTACK_HIT, Move.BLOCK):
        return (new Reservoir.Delta(0, kEnergyDeltaAttack, 0),
            new Reservoir.Delta(0, 0, kFocusDeltaBlock));
      case (Move.NEUTRAL, Move.ATTACK_HIT):
      case (Move.ATTACK_PREP_1, Move.ATTACK_HIT):
      case (Move.ATTACK_PREP_2, Move.ATTACK_HIT):
        return (new Reservoir.Delta(kHealthDeltaAttackSuccess, 0, 0),
            new Reservoir.Delta(0, kEnergyDeltaAttack, 0));
      case (Move.ATTACK_HIT, Move.ATTACK_HIT):
        return (
            new Reservoir.Delta(
                kHealthDeltaAttackSuccess, kEnergyDeltaAttack, 0),
            new Reservoir.Delta(
                kHealthDeltaAttackSuccess, kEnergyDeltaAttack, 0));
      default:
        throw new Exception();
    }
  }

  public static void Do(Combatant a, Combatant b) {
    var move_count = 0;
    const int kMaxMoveCount = 1000;
    while (a.IsAlive() && b.IsAlive() && move_count < kMaxMoveCount) {
      var (a_move, b_move) = (a.GetMove(b), b.GetMove(a));
      Console.WriteLine(move_count + " a-" + a_move + " b-" + b_move);
      if (!Transitions.IsValid(a.move, a_move) ||
          !Transitions.IsValid(b.move, b_move)) {
        throw new InvalidOperationException("");
      }
      var (a_delta, b_delta) =
          ResolveReservoirDeltas(a_move, b_move);
      a.ApplyResult(a_move, a_delta);
      b.ApplyResult(b_move, b_delta);
      move_count += 1;
    }
    if (!a.IsAlive()) { Console.WriteLine("a died"); }
    if (!b.IsAlive()) { Console.WriteLine("b died"); }
    if (move_count == kMaxMoveCount) { Console.WriteLine("TIMEOUT"); }
  }
}