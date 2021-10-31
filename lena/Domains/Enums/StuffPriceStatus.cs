using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum StuffPriceStatus : short
  {
    None = 0,
    Current = 1,
    Check = 2,
    Accept = 4,
    Reject = 8,
    Archived = 16,
    Deleted = 32
  }
}
