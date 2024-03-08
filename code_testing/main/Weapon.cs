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

// units: frequency
// Empty hand = 1.0
using Speed = double;

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
  // In one handed mode -- hand holds at 0.
  // In two handed mode -- 2nd hand assists by holding at any point closest to the payload (can be 0 as well). 

  public DistanceFeet min_payload_range { get; }
  public DistanceFeet max_payload_range { get; }
  public WeightPounds payload_weight { get; }
  public Sharpness sharpness { get; }

  public Speed swing_speed_1h { get; }
  public Speed swing_speed_2h { get; }

  protected Weapon(
      DistanceFeet min_payload_range_,
      DistanceFeet max_payload_range_,
      WeightPounds payload_weight_,
      Sharpness sharpness_, Speed swing_speed_1h_, Speed swing_speed_2h_) {
    min_payload_range = min_payload_range_;
    max_payload_range = max_payload_range_;
    payload_weight = payload_weight_;
    sharpness = sharpness_;
    swing_speed_1h = swing_speed_1h_;
    swing_speed_2h = swing_speed_2h_;
  }
}

// Uses: 
public class Dagger() : Weapon(0, 1, 1, Sharpness.SHARP, 1.0, 1.0);

public class ShortSword() : Weapon(0, 2.5, 2.5, Sharpness.SHARP, 0.8, 0.85);

public class LongSword() : Weapon(0, 3.5, 3.5, Sharpness.SHARP, 0.65, 0.725);

public class BastardSword() : Weapon(0, 4.5, 4.5, Sharpness.SHARP, 0.55, 0.675);

public class Claymore() : Weapon(0, 5.5, 5.5, Sharpness.SHARP, 0.2, 0.6);

public class Spear() : Weapon(8, 9, 2, Sharpness.SHARP, 0.2, 0.5);

// 2h
// Destroy shields/armor+
// Blocking--
public class BattleAxe() : Weapon(4, 6, 10, Sharpness.SHARP, 0, 0.3);

// 1h blunt.
// Destroy shields/armor+
// Blocking-
public class Mace() : Weapon(2, 4, 10, Sharpness.BLUNT, 0.5, 0.65);

// 2h blunt
// Destroy shields/armor++
// Blocking--
public class Hammer() : Weapon(4, 6, 20, Sharpness.BLUNT, 0, 0.2);

// TODO:
// * Pike
// * Flail
// * Staff
// * Halberd