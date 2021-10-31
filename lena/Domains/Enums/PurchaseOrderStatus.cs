using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum PurchaseOrderStatus : short
  {
    None = 0,
    NotAction = 1,
    Cargoing = 2,
    Cargoed = 4,
    Receipting = 8,
    Receipted = 16,
    QualityControling = 32,
    QualityControled = 64
  }
}
