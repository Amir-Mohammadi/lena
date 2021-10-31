using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProductionOrderStatus : byte
  {
    None = 0,
    NotAction = 1,
    ProductionMaterialRequested = 2,
    InProduction = 4,
    Produced = 8,
    Finished = 16
  }
}
