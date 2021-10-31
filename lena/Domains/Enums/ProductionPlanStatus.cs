using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProductionPlanStatus : byte
  {
    None = 0,
    NotAction = 1,
    Scheduling = 2,
    Scheduled = 4,
    InProduction = 8,
    Produced = 16
  }
}
