using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum PurchaseReportStatus : short
  {
    None = 0,
    Waiting = 1,
    Accepted = 2,
    Rejected = 4,
    Ordering = 8,
    Ordered = 16,
    Cargoing = 32,
    Cargoed = 64,
    Receipting = 128,
    Receipted = 256,
    QualityControling = 512,
    QualityControled = 1024
  }
}
