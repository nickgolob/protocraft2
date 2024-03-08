namespace main;

using LevelType = int;

public class Reservoir(
    LevelType health_max_,
    LevelType energy_max_,
    LevelType focus_max_) {
  // When health is 0 you die.
  //   TODO -- side effects? Wounded state? Maybe that can be incorporated into energy
  public Field health { get; } = new(health_max_);

  // Ability to move fast and hit hard.
  // * Lower energy has a progressive effect.
  // * Blocking is cheaper than attacking.
  public Field energy { get; } = new(energy_max_);

  public Field focus { get; } = new(focus_max_);

  public class Field(LevelType max_) {
    public LevelType max { get; set; } = max_;
    public LevelType current { get; set; } = max_;
  }

  public readonly struct Delta(
      LevelType health_delta_,
      LevelType energy_delta_,
      LevelType focus_delta_) {
    public LevelType health_delta { get; } = health_delta_;
    public LevelType energy_delta { get; } = energy_delta_;
    public LevelType focus_delta { get; } = focus_delta_;
  }

  public static Reservoir operator +(Reservoir reservoir, Delta delta) {
    reservoir.health.current += delta.health_delta;
    reservoir.energy.current += delta.energy_delta;
    reservoir.focus.current += delta.focus_delta;
    return reservoir;
  }
}