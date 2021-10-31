using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProductionScheduleStatus : byte
  {
    None = 0,
    NotAction = 1,
    InProduction = 2,
    Produced = 4,
    Finished = 8

  }
}
