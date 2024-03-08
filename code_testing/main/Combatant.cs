namespace main;

public abstract class Combatant(string name_, Reservoir reservoir_) {
  public readonly string name = name_;
  public Reservoir reservoir = reservoir_;

  public Weapon? weapon;
  public Shield? shield;

  public Move move = Move.NEUTRAL;

  protected Combatant(string name_) : this(
      name_,
      new Reservoir(10, 10, 10)) { }

  public abstract Move GetMove(Combatant opponent);

  public void ApplyResult(Move move_update, Reservoir.Delta delta) {
    move = move_update;
    reservoir += delta;
  }

  public bool IsAlive() {
    return reservoir.health.current > 0;
  }
}