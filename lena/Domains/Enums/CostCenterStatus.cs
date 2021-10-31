using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum CostCenterStatus : byte
  {
    NotAction,
    Accepted,
    Rejected,
  }
}
