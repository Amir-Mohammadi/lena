using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum CargoItemStatus : short
  {
    None = 0,
    NotAction = 1,
    Receipting = 2,
    Receipted = 4,
    QualityControling = 8,
    QualityControled = 16
  }
}
