namespace main;

// -----------------------------------------------------------------------------
// todo -- references
using AreaFeetSqrd = double;
using WeightPounds = double;
using Hardness = double;

public class Shield {
  public AreaFeetSqrd area { get; }
  public WeightPounds weight { get; }

  protected Shield(AreaFeetSqrd area_, WeightPounds weight_) {
    area = area_;
    weight = weight_;
  }
}

public class Buckler() : Shield(2, 2) { }

public class MediumShield() : Shield(9, 9) { }

public class TowerShield() : Shield(20, 20) { }