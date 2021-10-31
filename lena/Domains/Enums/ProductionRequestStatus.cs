using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProductionRequestStatus : short
  {
    None = 0,
    NotAction = 1,
    Planning = 2,
    Planned = 4,
    Scheduling = 8,
    Scheduled = 16,
    InProduction = 32,
    Produced = 64
  }
}
