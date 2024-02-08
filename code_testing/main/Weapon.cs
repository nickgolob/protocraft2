using System.Data.SqlTypes;

namespace main;

// NEED something that works for:
// Swords:
//   Dagger
//   short sword
//   long sword
//   bastard sword
//   claymore
// Spear / Pike
// Axe:
// Mace:
//   small / large.
// Staff
// Halberd?
// Flail?

// Parts:
//   payload
//   hilt

// Common attributes:
//   weight of payload
//   *sharpness of payload* ?
//   weight of hilt? 
// *hardness* of types

// -----------------------------------------------------------------------------
// arms length = 3 ft
using DistanceFeet = double;
// short sword blade = 5 lbs. Battle axe weight = 20 lbs?
using WeightPounds = double;

// wood == 1.0. Steel = 2.0?
using Hardness = double;

// blunt == 0.0.  Infinite needle = 1.0.
// Can cut things open compared to `Hardness`
public enum Sharpness {
  BLUNT,
  SHARP
}

// Does not account for user reach, which should be ~~[-1ft, 3ft]
public class Weapon {
  // hilt_range = [0, min_payload_range] (Flail is an exception?)
  public DistanceFeet min_payload_range { get; }
  public DistanceFeet max_payload_range { get; }
  public WeightPounds payload_weight { get; }
  public Sharpness sharpness { get; }

  protected Weapon(
      DistanceFeet min_payload_range_,
      DistanceFeet max_payload_range_,
      WeightPounds payload_weight_,
      Sharpness sharpness_) {
    min_payload_range = min_payload_range_;
    max_payload_range = max_payload_range_;
    payload_weight = payload_weight_;
    sharpness = sharpness_;
  }
}

// Uses: 
public class Dagger() : Weapon(0, 1, 2, Sharpness.SHARP);

public class ShortSword() : Weapon(0, 2.5, 5, Sharpness.SHARP);

public class LongSword() : Weapon(0, 3.5, 7, Sharpness.SHARP);

public class BastardSword() : Weapon(0, 4.5, 9, Sharpness.SHARP);

public class Claymore() : Weapon(0, 5.5, 11, Sharpness.SHARP);

public class Spear() : Weapon(8, 9, 2, Sharpness.SHARP);

// Destroy shields/armor+
// Blocking--
public class BattleAxe() : Weapon(4, 6, 20, Sharpness.SHARP);

// Destroy shields/armor+
// Blocking-
public class Mace() : Weapon(3, 5, 20, Sharpness.BLUNT);

// Destroy shields/armor++
// Blocking--
public class Hammer() : Weapon(4, 6, 20, Sharpness.BLUNT);\

// TODO:
// * Flail
// * Staff
// * Halberd