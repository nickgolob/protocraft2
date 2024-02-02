using System.Collections.Specialized;

namespace main;

public class Reservoir(int health_, int energy_, int focus_) {
    public int health { get; } = health_;
    public int energy { get; } = energy_;
    public int focus { get; } = focus_;

    public static Reservoir operator +(Reservoir a, Reservoir b) =>
        new(a.health + b.health, a.energy + b.energy, a.focus + b.focus);
}

public abstract class Combatant(string name_) {
    public readonly string name = name_;
    public Move move = Move.NEUTRAL;
    public Reservoir reservoir = new(10, 10, 10);

    public abstract Move GetMove(Combatant opponent);

    public void ApplyResult(Move move_update, Reservoir res_delta) {
        move = move_update;
        reservoir += res_delta;
    }

    public bool IsAlive() {
        return reservoir.health > 0;
    }
}