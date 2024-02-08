using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace main;


public struct Reservoir {
    public struct 
}
public struct Reservoir(Reservoir.Attributes max_) {
    public Attributes max = max_;
    public Attributes current = max_;

    public struct Attributes(int health_, int energy_, int focus_) {
        // When health is 0 you die.
        //   TODO -- side effects? Wounded state? Maybe that can be incorporated into energy
        public int health { get; } = health_;

        // Ability to move fast and hit hard.
        // * Lower energy has a progressive effect.
        // * Blocking is cheaper than attacking.
        public int energy { get; } = energy_;


        public int focus { get; } = focus_;

        public static Attributes operator +(Attributes a, Attributes b) =>
            new(a.health + b.health, a.energy + b.energy, a.focus + b.focus);
    }
}

public abstract class Combatant(string name_, Reservoir reservoir_) {
    public readonly string name = name_;
    public Move move = Move.NEUTRAL;
    public Reservoir reservoir = reservoir_;

    public Combatant(string name_) : this(name_,
        new Reservoir(new Reservoir.Attributes(10, 10, 10))) { }

    public abstract Move GetMove(Combatant opponent);

    public void ApplyResult(Move move_update, Reservoir res_delta) {
        move = move_update;
        reservoir += res_delta;
    }

    public bool IsAlive() {
        return reservoir.health > 0;
    }
}