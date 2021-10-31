using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum BankOrderStatus : byte
  {
    None = 0,
    Incomplete = 1,
    Completed = 2
  }
}
